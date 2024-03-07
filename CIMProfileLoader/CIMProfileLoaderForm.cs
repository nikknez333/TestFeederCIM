using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDS.RDF;
using VDS.RDF.Parsing.Handlers;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using System.IO;
using CIMProfileLoader.Parser;
using CIMProfileLoader.Core;
using VDS.RDF.Ontology;
using VDS.RDF.Writing.Formatting;
using CIMProfileLoader.Storage;
using VDS.RDF.Query;
using VDS.RDF.Query.Inference;
using CIMProfileLoader.Formatter;
using BenchmarkDotNet.Running;

namespace CIMProfileLoader
{
    public partial class CIMProfileLoaderForm : Form
    {
        private OntologyGraph SchemaGraph = new OntologyGraph();
        private Graph DataGraph = new Graph();
        private List<string> StoredGraphs = new List<string>();
        private RDFFileHandler rdfHandler = new RDFFileHandler();
        private ITripleFormatter formatter = new NTriplesFormatter();
        private IResultFormatter sparqlResultFromatter = new SPARQLResultFormatter();
        private FusekiStorageAPI fusekiStore = new FusekiStorageAPI();
        private string rdfsFileLoaded = string.Empty;
        private Dictionary<string, List<string>> datasetsWithGraphs = new Dictionary<string, List<string>>();

        public CIMProfileLoaderForm()
        {
            InitializeComponent();
            rdfHandler.Message += RdfParser_Message;
            comboBoxQueryTypeOfEquipment.Items.Add("Conducting Equipment");
            comboBoxQueryTypeOfEquipment.Items.Add("Switch");
            RefreshControls();
        }

        //TO DO: 1.check paths
        private void RefreshControls()
        {
            //LoadProfile Button
            bool IsCIMProfileSelected = !string.IsNullOrWhiteSpace(textBoxCIMProfile.Text);
            buttonLoadProfile.Enabled = IsCIMProfileSelected;

            //ApplyRDFSInferenceOverDataset Button
            buttonApplyRDFSInferenceOverDataset.Enabled = !SchemaGraph.IsEmpty;

            //LoadData Button
            bool IsRDFXMLSelected = !string.IsNullOrWhiteSpace(textBoxRDFFile.Text);
            buttonLoadData.Enabled = IsRDFXMLSelected;

            buttonStopJenaStorage.Enabled = false;

            //ListGraphs Button
            buttonListGraphs.Enabled = comboBoxRDFDatasets.SelectedItem != null;

            //LoadGraph Button
            buttonLoadGraph.Enabled = comboBoxRDFGraphs.SelectedItem != null;

            //UpdateGraph Button
            buttonUpdateGraph.Enabled = (comboBoxRDFGraphs.SelectedItem != null)
                && (checkBoxAddTriple.Checked || checkBoxDeleteTriple.Checked);

            //DeleteGraph Button
            buttonDeleteGraph.Enabled = comboBoxRDFGraphs.SelectedItem != null;

            //SaveGraph Button
            buttonSaveGraph.Enabled = IsRDFXMLSelected;
        
            //Add/Remove triple check box
            if (checkBoxAddTriple.Checked)
                checkBoxDeleteTriple.Enabled = false;
            else if (checkBoxDeleteTriple.Checked)
                checkBoxAddTriple.Enabled = false;

            //Query - NumOfClasses
            buttonQueryNumOfClasses.Enabled = comboBoxRDFDatasets.SelectedItem != null && comboBoxRDFGraphs.SelectedItem != null;

            //Query - Class-Attribute Info
            buttonQueryAllClassesAttr.Enabled = comboBoxRDFDatasets.SelectedItem != null && comboBoxRDFGraphs.SelectedItem != null;

            //Query - Get Equipment
            buttonQueryGetTypeOfEquipment.Enabled = comboBoxRDFDatasets.SelectedItem != null && comboBoxRDFGraphs.SelectedItem != null;

            //Query - Get Components
            buttonQueryConnectivityNodeComponents.Enabled = comboBoxRDFDatasets.SelectedItem != null && comboBoxRDFGraphs.SelectedItem != null;

            //Query - Get Data
            buttonQueryGetComponentInfo.Enabled = comboBoxRDFDatasets.SelectedItem != null && comboBoxRDFGraphs.SelectedItem != null;

            if(comboBoxRDFDatasets.SelectedItem != null)
            {
                string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();

                if (comboBoxRDFGraphs.SelectedItem == null)
                {
                    comboBoxRDFGraphs.Items.Clear();
                    comboBoxRDFGraphs.SelectedItem = null;
                    comboBoxRDFGraphs.Text = string.Empty;
                    if (datasetsWithGraphs.ContainsKey(selectedDataset))
                    {
                        comboBoxRDFGraphs.Enabled = true;
                        comboBoxRDFGraphs.Items.AddRange(datasetsWithGraphs[selectedDataset].ToArray());
                    }
                }
            }
            /*if (StoredGraphs.Count > 0)
            {
                comboBoxRDFGraphs.Enabled = true;

                foreach (String graphName in StoredGraphs)
                {
                    if(!comboBoxRDFGraphs.Items.Contains(graphName))
                        comboBoxRDFGraphs.Items.Add(graphName);
                }
            }*/

            //buttonStartJenaStorage.Enabled = IsCIMProfileSelected && IsRDFXMLSelected;
        }

        #region Operations

        private void ShowOpenCIMProfileFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open CIM Profile File..";
            openFileDialog.Filter = "CIM-RDFS FILES|*.rdfs;*.legacy-rdfs;*.xml|All Files|*.*";
            openFileDialog.RestoreDirectory = true;

            DialogResult dialogResponse = openFileDialog.ShowDialog(this);

            if(dialogResponse == DialogResult.OK)
            {
                textBoxCIMProfile.Text = openFileDialog.FileName;
            }

            RefreshControls();
        }

        private void ShowOpenRDFXMLFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open RDF/XML File..";
            openFileDialog.Filter = "RDF/XML FILES|*.rdf;*.xml|All Files|*.*";
            openFileDialog.RestoreDirectory = true;

            DialogResult dialogResponse = openFileDialog.ShowDialog(this);

            if(dialogResponse == DialogResult.OK)
            {
                textBoxRDFFile.Text = openFileDialog.FileName;
            }

            RefreshControls();
        }

        private void LoadRDFFile()
        {
            try
            {
                richTextBoxReport.Clear();

                if (buttonLoadProfile.Capture)
                {
                    SchemaGraph = (OntologyGraph)rdfHandler.LoadRDF(textBoxCIMProfile.Text);
                    rdfsFileLoaded = textBoxCIMProfile.Text;
                    PrintFilePreview(SchemaGraph);
                }
                else if(buttonLoadData.Capture)
                {
                    DataGraph = (Graph)rdfHandler.LoadRDF(textBoxRDFFile.Text);
                    PrintFilePreview(DataGraph);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(string.Format("An error occured\n\n{0}", e.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            RefreshControls();
        }

        private void ResetFusekiConfig()
        {
            richTextBoxReport.Clear();

            string fusekiConfigPreview = fusekiStore.ResetFusekiConfig();

            if (!string.IsNullOrEmpty(fusekiConfigPreview))
                richTextBoxReport.AppendText("Fuseki config reseted: \n\n");

            richTextBoxReport.AppendText(fusekiConfigPreview);
        }

        private void ApplyRDFSInferenceOverFusekiDataset()
        {
            try
            {
                richTextBoxReport.Clear();

                string fusekiConfigPreview = fusekiStore.UpdateFusekiConfig(rdfsFileLoaded);

                if(!fusekiConfigPreview.StartsWith("ERROR:"))
                    richTextBoxReport.AppendText("Fuseki config updated:\n\n");

                richTextBoxReport.AppendText(fusekiConfigPreview);
            }
            catch(RdfException e)
            {
                richTextBoxReport.AppendText("ERROR: " + e.Message);
            }
        }

        private void StartJenaStorage()
        {
            richTextBoxReport.Clear();

            try
            {
                fusekiStore.StartFusekiStore();

                string errorMsg = string.Empty;
                string datasetInfo = string.Empty;

                List<string> datasets = fusekiStore.GetAllDatasetsFromFuseki(ref errorMsg, ref datasetInfo);

                if (!datasets.Any() && !string.IsNullOrEmpty(errorMsg))
                {
                    richTextBoxReport.AppendText(errorMsg);
                }
                else
                {
                    richTextBoxReport.AppendText(String.Format("Successfully started Apache Jena Storage on: http://localhost:3030/ \n\ndatasets info:\n"));
                    foreach (string dataset in datasets)
                    {
                        datasetsWithGraphs.Add(dataset, new List<string>());
                    }
                    comboBoxRDFDatasets.Items.AddRange(datasetsWithGraphs.Keys.ToArray());
                    richTextBoxReport.AppendText(datasetInfo);
                }

                buttonStartJenaStorage.Enabled = false;
                buttonStopJenaStorage.Enabled = true;
            }
            catch(Exception e)
            {
                string errorMsg = String.Format("Could not start Jena Fuseki storage server, ERROR: {0}", e.Message);
                richTextBoxReport.AppendText(errorMsg);
            }
        }

        private void StopJenaStorage()
        {
            richTextBoxReport.Clear();

            try
            {
                fusekiStore.StopFusekiStore();

                comboBoxRDFDatasets.Items.Clear();

                richTextBoxReport.AppendText("Apache Jena Storage is now offline.");

                buttonStartJenaStorage.Enabled = true;
                buttonStopJenaStorage.Enabled = false;
            }
            catch(Exception e)
            {
                string errorMsg = String.Format("Could not stop Jena Fuseki storage server, ERROR: {0}", e.Message);
                richTextBoxReport.AppendText(errorMsg);
            }
        }

        private void ListRDFGraphsForGivenDataset()
        {
            richTextBoxReport.Clear();
            string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();
            string errorMessage = string.Empty;
            
            if (fusekiStore.IsFusekiOnline(ref errorMessage))
            {
                List<Uri> graphURIs = fusekiStore.ListGraphs(selectedDataset).ToList();
                richTextBoxReport.AppendText($"Graphs from {selectedDataset} dataset: \n\n");
                foreach (Uri graphURI in graphURIs)
                {
                    richTextBoxReport.AppendText("\t" + graphURI.ToString() + "\n\n");
                }
            }
            else
            {
                richTextBoxReport.AppendText($"Jena Storage is offline, ERROR: {errorMessage}");
            }
        }
        private void LoadGraphForPreview()
        {
            richTextBoxReport.Clear();

            string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();
            string selectedGraph = comboBoxRDFGraphs.SelectedItem.ToString();
            string errorMessage = string.Empty;

            if (fusekiStore.IsFusekiOnline(ref errorMessage))
            {
                Graph graphToLoad = fusekiStore.LoadGraph(selectedDataset,selectedGraph);
                richTextBoxReport.AppendText($"Graph {selectedGraph} from dataset {selectedDataset} was succesfully loaded.\n\n");

                PrintFilePreview(graphToLoad);
            }
            else
            {
                richTextBoxReport.AppendText($"\tFuseki storage is currently offline! ERROR: {errorMessage}");
            }
        }

        private void UpdateGraphFromFusekiDataset()
        {
            richTextBoxReport.Clear();

            string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();
            string selectedGraph = comboBoxRDFGraphs.SelectedItem.ToString();
            string errorMessage = string.Empty;

            if (fusekiStore.IsFusekiOnline(ref errorMessage))
            {
                bool updated = fusekiStore.UpdateGraph(selectedDataset,selectedGraph, textBoxTripleSubject.Text, textBoxTriplePredicate.Text, textBoxTripleObject.Text, checkBoxAddTriple.Checked, checkBoxDeleteTriple.Checked);

                if (updated)
                {
                    richTextBoxReport.AppendText("Graph " + selectedGraph + " was successfully updated in fuseki triple store, you can preview changes clicking on Load Graph method.");
                }
                else
                    richTextBoxReport.AppendText("Could not update graph " + selectedGraph + "!");
            }
            else
            {
                richTextBoxReport.AppendText("ERROR: Fuseki storage is currently offline!");
            }

        }

        private void DeleteGraphFromFusekiDataset()
        {
            richTextBoxReport.Clear();

            string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();
            string selectedGraph = comboBoxRDFGraphs.SelectedItem.ToString();
            string errorMessage = string.Empty;

            if (fusekiStore.IsFusekiOnline(ref errorMessage))
            {
                bool deleted = fusekiStore.DeleteGraph(selectedDataset, selectedGraph);

                if (deleted)
                {
                    comboBoxRDFGraphs.Items.Remove(selectedGraph);
                    comboBoxRDFGraphs.SelectedItem = null;
                    comboBoxRDFGraphs.Text = string.Empty;
                    datasetsWithGraphs[selectedDataset].Remove(selectedGraph);
                    if(comboBoxRDFGraphs.Items.Count == 0)
                    {
                        comboBoxRDFGraphs.Enabled = false;
                        StoredGraphs.Remove(selectedGraph);
                        datasetsWithGraphs[selectedDataset].Remove(selectedGraph);
                        RefreshControls();
                    }
                    richTextBoxReport.AppendText("Graph " + selectedGraph + " was successfully deleted from fuseki triple store!");
                }
                else
                    richTextBoxReport.AppendText("Could not delete graph " + selectedGraph + "delete operation is not supported by store!");
            }
            else
            {
                richTextBoxReport.AppendText("ERROR: Fuseki storage is currently offline!");
            }
        }

        private void SaveGraphToFusekiDataset()
        {
            richTextBoxReport.Clear();
            
            string errorMessage = string.Empty;

            if (comboBoxRDFDatasets.SelectedItem != null)
            {
                string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();
                if (!DataGraph.IsEmpty)
                {
                    if (fusekiStore.IsFusekiOnline(ref errorMessage))
                    {
                        bool saved = fusekiStore.SaveGraph(selectedDataset, DataGraph, textBoxRDFFile.Text);

                        if (saved)
                        {
                            richTextBoxReport.AppendText("Data graph " + textBoxRDFFile.Text + " was sucessfully saved to fuseki triple store!");
                            string graphName = Path.GetFileNameWithoutExtension(textBoxRDFFile.Text);
                            datasetsWithGraphs[selectedDataset].Add(graphName);
                            //StoredGraphs.Add(graphName);
                        }
                        else
                            richTextBoxReport.AppendText("Couldn't save data graph " + textBoxRDFFile.Text + " as he is already saved.Please make sure you load data graph before proceeding!");
                    }
                    else
                    {
                        richTextBoxReport.AppendText("ERROR: Fuseki storage is currently offline!");
                    }

                }
                else
                {
                    richTextBoxReport.AppendText("Select RDF/XML data file to proceed with saving the graph.");
                }
            }
            else
            {
                richTextBoxReport.AppendText("Select specific dataset to proceed with saving the graph.");
            }

            RefreshControls();
        }

        private void RdfParser_Message(object sender, string message)
        {
            richTextBoxReport.AppendText(message + "\n\n");
        }

        private void PrintFilePreview(IGraph RDFGraph)
        {
            richTextBoxReport.AppendText("Triples data: \n\n");
            if(!RDFGraph.IsEmpty)
            {
                foreach (Triple t in RDFGraph.Triples)
                {
                    richTextBoxReport.AppendText("\t" + t.ToString(formatter));
                    richTextBoxReport.AppendText("\n");
                }
            }
        }

        #region Query

        private void GetClassesWithAttributesFromFuseki()
        {
            richTextBoxReport.Clear();

            string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();
            string selectedGraph = comboBoxRDFGraphs.SelectedItem.ToString();
            string errorMessage = string.Empty;

            if (fusekiStore.IsFusekiOnline(ref errorMessage))
            {
                SparqlResultSet rset = fusekiStore.GetClassesWithAttributes(textBoxQueryClassAttInfo.Text,selectedDataset,selectedGraph);

                SPARQLResultFormatter sparqlFormatter = new SPARQLResultFormatter();
                foreach(SparqlResult result in rset)
                {
                    sparqlFormatter.Format(result);
                }

                //string formattedData = sparqlFormatter.Format(result));

                richTextBoxReport.AppendText(sparqlFormatter.PrintFormattedData());
            }
            else
            {
                richTextBoxReport.AppendText("\tERROR: Fuseki storage is currently offline!");
            }
        }

        private void GetNumOfClassesFromFuseki()
        {
            richTextBoxReport.Clear();
            string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();
            string selectedGraph= comboBoxRDFGraphs.SelectedItem.ToString();
            string errorMessage = string.Empty;

            if(fusekiStore.IsFusekiOnline(ref errorMessage))
            {
                SparqlResultSet rset = fusekiStore.GetNumOfClasses(selectedDataset, selectedGraph);
                string classesNum = "No. of classes";
                string resultValue = rset[0]["numOfClasses"].ToString();
                richTextBoxReport.AppendText($"{classesNum} = {resultValue}");
            }
            else
            {
                richTextBoxReport.AppendText("\tERROR: Fuseki storage is currently offline!");
            }
        }

        public void GetTypeOfEquipmentFromFuseki()
        {
            richTextBoxReport.Clear();

            string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();
            string selectedGraph = comboBoxRDFGraphs.SelectedItem.ToString();
            string errorMesage = string.Empty;

            if(fusekiStore.IsFusekiOnline(ref errorMesage))
            {
                SparqlResultSet rset = fusekiStore.GetTypeOfEquipment(selectedDataset, selectedGraph, comboBoxQueryTypeOfEquipment.SelectedItem.ToString());
                richTextBoxReport.AppendText($"{comboBoxQueryTypeOfEquipment.SelectedItem.ToString()} present in {selectedGraph} is:\n\n");

                SPARQLResultFormatter sparqlFormatter = new SPARQLResultFormatter();
                foreach (SparqlResult result in rset)
                {
                    sparqlFormatter.Format(result);
                }

                richTextBoxReport.AppendText(sparqlFormatter.PrintFormattedInstanceData());
            }
            else
            {
                richTextBoxReport.AppendText("\tERROR: Fuseki storage is currently offline!");
            }
        }

        public void GetConnectivityNodeComponentsFromFuseki()
        {
            richTextBoxReport.Clear();

            string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();
            string selectedGraph = comboBoxRDFGraphs.SelectedItem.ToString();
            string errorMesage = string.Empty;

            if(fusekiStore.IsFusekiOnline(ref errorMesage))
            {
                SparqlResultSet rset = fusekiStore.GetConnectivityNodeComponents(selectedDataset, selectedGraph, textBoxQueryConnectivityNode.Text);
                richTextBoxReport.AppendText($"Components belonging to component with ID {textBoxQueryConnectivityNode.Text} are: \n\n");

                SPARQLResultFormatter sparqlFormatter = new SPARQLResultFormatter();
                foreach (SparqlResult result in rset)
                {
                    sparqlFormatter.Format(result);
                }

                richTextBoxReport.AppendText(sparqlFormatter.PrintFormattedInstanceData());
            }
            else
            {
                richTextBoxReport.AppendText("\tERROR: Fuseki storage is currently offline!");
            }
        }

        public void GetComponentInfoFromFuseki()
        {
            richTextBoxReport.Clear();

            string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();
            string selectedGraph = comboBoxRDFGraphs.SelectedItem.ToString();
            string errorMesage = string.Empty;

            if (fusekiStore.IsFusekiOnline(ref errorMesage))
            {
                SparqlResultSet rset = fusekiStore.GetComponentInfo(selectedDataset, selectedGraph, textBoxQueryComponentID.Text);
                richTextBoxReport.AppendText($"Component data: \n\n");

                SPARQLResultFormatter sparqlFormatter = new SPARQLResultFormatter();
                foreach (SparqlResult result in rset)
                {
                    sparqlFormatter.Format(result);
                }

                richTextBoxReport.AppendText(sparqlFormatter.PrintFormattedInstanceData());
            }
            else
            {
                richTextBoxReport.AppendText("\tERROR: Fuseki storage is currently offline!");
            }
        }

        #endregion

        #endregion Operations


        #region EventHandlers

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            ShowOpenCIMProfileFileDialog();
        }

        private void buttonBrowseRDF_Click(object sender, EventArgs e)
        {
            ShowOpenRDFXMLFileDialog();
        }

        private void buttonLoadProfile_Click(object sender, EventArgs e)
        {
            LoadRDFFile();
        }

        private void buttonResetConfig_Click(object sender, EventArgs e)
        {
            ResetFusekiConfig();
        }

        private void buttonApplyRDFSInferenceOverDataset_Click(object sender, EventArgs e)
        {
            ApplyRDFSInferenceOverFusekiDataset();
        }

        private void buttonLoadRDF_Click(object sender, EventArgs e)
        {
            LoadRDFFile();
        }

        private void buttonStartJenaAndApplyReasoner_Click(object sender, EventArgs e)
        {
            StartJenaStorage();
        }

        private void buttonStopJenaStorage_Click(object sender, EventArgs e)
        {
            StopJenaStorage();
        }

        private void buttonListGraphs_Click(object sender, EventArgs e)
        {
            ListRDFGraphsForGivenDataset();
        }
        private void buttonLoadGraph_Click(object sender, EventArgs e)
        {
            LoadGraphForPreview();
        }

        private void buttonUpdateGraph_Click(object sender, EventArgs e)
        {
            UpdateGraphFromFusekiDataset();
        }

        private void buttonDeleteGraph_Click(object sender, EventArgs e)
        {
            DeleteGraphFromFusekiDataset();
        }

        private void buttonSaveGraph_Click(object sender, EventArgs e)
        {
            SaveGraphToFusekiDataset();
        }

        private void buttonQueryAllClassesAttr_Click(object sender, EventArgs e)
        {
            GetClassesWithAttributesFromFuseki();
        }

        private void buttonQueryNumOfClasses_Click(object sender, EventArgs e)
        {
            GetNumOfClassesFromFuseki();
        }

        private void buttonQueryGetTypeOfEquipment_Click(object sender, EventArgs e)
        {
            GetTypeOfEquipmentFromFuseki();
        }

        private void buttonQueryConnectivityNodeComponents_Click(object sender, EventArgs e)
        {
            GetConnectivityNodeComponentsFromFuseki();
        }

        private void buttonQueryGetComponentInfo_Click(object sender, EventArgs e)
        {
            GetComponentInfoFromFuseki();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBoxAddTriple_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAddTriple.Checked)
            {
                checkBoxDeleteTriple.Enabled = false;
                if (comboBoxRDFGraphs.SelectedItem != null)
                    buttonUpdateGraph.Enabled = true;
            }
            else
            {
                checkBoxDeleteTriple.Enabled = true;
                buttonUpdateGraph.Enabled = false;
            }
        }

        private void checkBoxDeleteTriple_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDeleteTriple.Checked)
            {
                checkBoxAddTriple.Enabled = false;
                if (comboBoxRDFGraphs.SelectedItem != null)
                    buttonUpdateGraph.Enabled = true;
            }
            else
            {
                checkBoxAddTriple.Enabled = true;
                buttonUpdateGraph.Enabled = false;
            }
        }

        private void comboBoxRDFGraphs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRDFGraphs.SelectedItem != null)
            {
                buttonLoadGraph.Enabled = true;
                buttonDeleteGraph.Enabled = true;
            }
            if ((comboBoxRDFGraphs.SelectedItem != null) && (checkBoxAddTriple.Checked || checkBoxDeleteTriple.Checked))
            {
                buttonUpdateGraph.Enabled = true;
            }
            if (!checkBoxAddTriple.Checked && !checkBoxDeleteTriple.Checked)
            {
                buttonUpdateGraph.Enabled = false;
            }

            RefreshControls();
        }

        private void comboBoxRDFDatasets_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDataset = comboBoxRDFDatasets.SelectedItem.ToString();

            comboBoxRDFGraphs.Items.Clear();
            comboBoxRDFGraphs.SelectedItem = null;
            comboBoxRDFGraphs.Text = string.Empty;

            if (datasetsWithGraphs.ContainsKey(selectedDataset))
            {
                comboBoxRDFGraphs.Items.AddRange(datasetsWithGraphs[selectedDataset].ToArray());
            }
        }


        #endregion EventHandlers

    }
}
