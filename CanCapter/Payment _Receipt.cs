using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Word;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CanCapter
{
    internal class Payment__Receipt
    {
        public static void print(string filier, string matiere, string mois, string montantPaye, string montantRester, string recuN, string fullName)
        {
            //...

            // Create a new Microsoft Word application
            Application wordApp = new Application();

            // Open the existing receipt template
            string receiptTemplate = Directory.GetCurrentDirectory() + @"\Template\Payment_Receipt_Template.docx";
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(receiptTemplate);

            // Add custom information to the receipt
            doc.Bookmarks["Filier"].Range.Text = filier;
            doc.Bookmarks["Date"].Range.Text = DateTime.Now.ToShortDateString();
            doc.Bookmarks["Matiere"].Range.Text = matiere;
            doc.Bookmarks["Mois"].Range.Text = mois;
            doc.Bookmarks["MontantP"].Range.Text = montantPaye;
            doc.Bookmarks["MontantR"].Range.Text = montantRester;
            doc.Bookmarks["Name"].Range.Text = fullName;
            doc.Bookmarks["RecuNumber"].Range.Text = recuN;

            // Save the receipt as a new Word document in a folder
            string folderPath = @"G:\Users\moami\Desktop\";
            string fileName = "Receipt_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx";
            doc.SaveAs2(folderPath + fileName);

            // Close the document and the Word application
            doc.Close();
            wordApp.Quit();


        }

        public static string printRecu(string filier, string matiere, string mois, string montantPaye, string montantRester, string recuN, string fullName,string mtTotal)
        {
            Guid guid = Guid.NewGuid();
            string folderPath = Directory.GetCurrentDirectory() + @"\Recus\";
            string fileName = "Receipt_" + guid.ToString() + DateTime.Now.ToString("yyyyMMddHH") + ".docx";
            string receiptTemplate = Directory.GetCurrentDirectory() + @"\Template\Payment_Receipt_Template_Recu.docx";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            try
            {
                //...

                // Create a new Microsoft Word application
                Application wordApp = new Application();

                // Open the existing receipt template
                Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(receiptTemplate);
                // Add custom information to the receipt
                doc.Bookmarks["Filier"].Range.Text = filier;
                doc.Bookmarks["Date"].Range.Text = DateTime.Now.ToShortDateString();
                doc.Bookmarks["Mois"].Range.Text = mois;
                doc.Bookmarks["MontantP"].Range.Text = montantPaye;
                doc.Bookmarks["MontantR"].Range.Text = montantRester;
                doc.Bookmarks["Name"].Range.Text = fullName;
                doc.Bookmarks["RecuNumber"].Range.Text = recuN;
                doc.Bookmarks["MontantTotal"].Range.Text = mtTotal;
                doc.Bookmarks["Matiere"].Range.Text = matiere;

                // Save the receipt as a new Word document in a folder

                doc.SaveAs2(folderPath + fileName);
                // Close the document and the Word application
                doc.Close();
                wordApp.Quit();

                return folderPath + fileName;
            }
            catch
            {
                try
                {
                    File.Copy(receiptTemplate, folderPath + fileName, true);

                    using (var doc = WordprocessingDocument.Open(folderPath + fileName, true))
                    {
                        MainDocumentPart mainPart = doc.MainDocumentPart;
                        var bookmarks = mainPart.Document.Body.Descendants<BookmarkStart>();
                        foreach (var bookmark in bookmarks)
                        {
                            if (bookmark.Name == "Filier")
                            {
                                var text = bookmark.Parent.Descendants<Text>().FirstOrDefault();
                                if (text != null)
                                {
                                    text.Text = text.Text + filier;
                                }
                            }
                            if (bookmark.Name == "Matiere")
                            {
                                var text = bookmark.Parent.Descendants<Text>().FirstOrDefault();
                                if (text != null)
                                {
                                    text.Text = text.Text + matiere;
                                }
                            }
                            else if (bookmark.Name == "Date")
                            {
                                var text = bookmark.Parent.Descendants<Text>().FirstOrDefault();
                                if (text != null)
                                {
                                    text.Text = DateTime.Now.ToShortDateString();
                                }
                            }
                            else if (bookmark.Name == "Mois")
                            {
                                var text = bookmark.Parent.Descendants<Text>().FirstOrDefault();
                                if (text != null)
                                {
                                    text.Text = text.Text + mois;
                                }
                            }
                            else if (bookmark.Name == "MontantP")
                            {
                                var text = bookmark.Parent.Descendants<Text>().FirstOrDefault();
                                if (text != null)
                                {
                                    text.Text = text.Text + montantPaye;
                                }
                            }
                            else if (bookmark.Name == "MontantR")
                            {
                                var text = bookmark.Parent.Descendants<Text>().FirstOrDefault();
                                if (text != null)
                                {
                                    text.Text = text.Text + montantRester;
                                }
                            }
                            else if (bookmark.Name == "Name")
                            {
                                var text = bookmark.Parent.Descendants<Text>().FirstOrDefault();
                                if (text != null)
                                {
                                    text.Text = text.Text + fullName;
                                }
                            }
                            else if (bookmark.Name == "RecuNumber")
                            {
                                var text = bookmark.Parent.Descendants<Text>().FirstOrDefault();
                                if (text != null)
                                {
                                    text.Text = text.Text + recuN;
                                }
                            }
                            else if (bookmark.Name == "MontantTotal")
                            {
                                var text = bookmark.Parent.Descendants<Text>().FirstOrDefault();
                                if (text != null)
                                {
                                    text.Text = text.Text + mtTotal;
                                }
                            }
                        }
                        mainPart.Document.Save();
                    }
                    return folderPath + fileName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public static void printWordfile(string filepath)
        {


            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "WINWORD.EXE";
                startInfo.Arguments = string.Format("/p {0}", filepath);

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
