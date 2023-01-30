using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace CanCapter
{
    internal class Payment__Receipt
    {
        public static void print(string filier, string matiere, string mois, string montantPaye, string montantRester, string recuN, string fullName )
        {
            //...

            // Create a new Microsoft Word application
            Application wordApp = new Application();

            // Open the existing receipt template
            string receiptTemplate = Directory.GetCurrentDirectory()+@"\Template\Payment_Receipt_Template.docx";
            Document doc = wordApp.Documents.Open(receiptTemplate);

            // Add custom information to the receipt
            doc.Bookmarks["Filier"].Range.Text = filier;
            doc.Bookmarks["Date"].Range.Text = DateTime.Now.ToShortDateString();
            doc.Bookmarks["Matiere"].Range.Text = matiere;
            doc.Bookmarks["Mois"].Range.Text = mois;
            doc.Bookmarks["MontantP"].Range.Text = montantPaye ;
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
        public static string printRecu(string filier, string mois, string montantPaye, string montantRester, string recuN, string fullName)
        {
            //...

            // Create a new Microsoft Word application
            Application wordApp = new Application();

            // Open the existing receipt template
            string receiptTemplate = Directory.GetCurrentDirectory() + @"\Template\Payment_Receipt_Template_Recu.docx";
            Document doc = wordApp.Documents.Open(receiptTemplate);

            // Add custom information to the receipt
            doc.Bookmarks["Filier"].Range.Text = filier;
            doc.Bookmarks["Date"].Range.Text = DateTime.Now.ToShortDateString();
            doc.Bookmarks["Mois"].Range.Text = mois;
            doc.Bookmarks["MontantP"].Range.Text = montantPaye;
            doc.Bookmarks["MontantR"].Range.Text = montantRester;
            doc.Bookmarks["Name"].Range.Text = fullName;
            doc.Bookmarks["RecuNumber"].Range.Text = recuN;

            // Save the receipt as a new Word document in a folder
            string folderPath = Directory.GetCurrentDirectory() + @"\Recus";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            string fileName = "Receipt_" + DateTime.Now.ToString("yyyyMMddHH") + ".docx";
            doc.SaveAs2(folderPath + fileName);
            
            // Close the document and the Word application
            doc.Close();
            wordApp.Quit();

            return folderPath + fileName;
        }

    }
}
