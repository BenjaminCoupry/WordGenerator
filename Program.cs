using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            GenererTexte("D:/lab/t.txt", 4, 3900);
        }
        static void BuildStats(string pathSource, int RefLenght, ref List<char> CharactersDic, ref int[,] Occurences, ref string txt)
        {
            //Extraction du texte
            txt = File.ReadAllText(@pathSource, Encoding.Default);
            // Listage des caractères
            CharactersDic = new List<char>();
            foreach (char c in txt)
            {
                if(!CharactersDic.Contains(c))
                {
                    CharactersDic.Add(c);
                }
                
            }
            int CharNum = CharactersDic.Count();
            //Construction Du Tableau (structure)
            Occurences = new int[(int)Math.Pow(CharNum,RefLenght),CharNum ];
            //Construction Du Tableau (contenu)
            int indexArray;
            for (int i=0; i<txt.Length-RefLenght;i++)
            {
                indexArray = 0;
                for(int j=0; j<RefLenght;j++)
                {
                    char c = txt[i + j];
                    int Index = CharactersDic.IndexOf(c);
                    indexArray += Index * (int)Math.Pow(CharNum, j);
                }
                char k = txt[i + RefLenght];
                Occurences[indexArray, CharactersDic.IndexOf(k)] += 1;
            }
            //Test
            //Les donnees sont stockees comme il suit "aa--,ba--,ca--...,ab--,bb--,cb--,..." est suivi par la lettre donnée, et ce n fois
            
        }
        static void CompleteStr(ref string str, int RefLenght, ref List<char> CharactersDic, ref int[,] Occurences, ref Random rand)
        {
            int CharNum = CharactersDic.Count();
            int RefIndex = 0;
            int k = 0;
            for(int i= str.Length - RefLenght; i< str.Length; i++)
            {
                char c = str[i];
                int Index = CharactersDic.IndexOf(c);
                RefIndex += Index * (int)Math.Pow(CharNum, k);
                k++;
            }
            int sumOccurences = 0;
            for(int j_=0; j_<CharNum; j_++)
            {
                sumOccurences += Occurences[RefIndex, j_];
            }
            int thresholdValue = rand.Next(0, sumOccurences);
            int j = 0;
            int w = 0;
            for (; j < CharNum; j++)
            {
                w += Occurences[RefIndex, j];
                if (w>thresholdValue)
                {
                    break;
                }
            }
            str = str + CharactersDic.ElementAt(j);
        }
        static void GenererTexte(string PATH, int LettersGroupLenght, int Completion)
        {
            Random random = new Random();
            List<char> CharactersDi = new List<char>();
            int[,] Occurence = new int[0, 0];
            string texte = "";
            BuildStats(PATH, LettersGroupLenght, ref CharactersDi, ref Occurence, ref texte);
            string genere = texte.Substring(0, LettersGroupLenght);
            for(int i=0; i<Completion;i++)
            {
                CompleteStr(ref genere, LettersGroupLenght, ref CharactersDi, ref Occurence, ref random);
            }
            Console.WriteLine(genere);
        }
    }
}
