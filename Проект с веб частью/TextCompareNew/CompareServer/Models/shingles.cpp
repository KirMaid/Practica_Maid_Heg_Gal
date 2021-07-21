#include <string.h>
#include <iostream>
#include <algorithm>
#include <iterator>
#include <sstream>
#include <locale>
#include <stdio.h>
#include <vector>
#include <omp.h>
#include "CCRC32.cpp"
#include <glob.h>

//#include "Shingle.h"

using namespace std;

namespace Shingles
{

class Shingle
{
    void string_tolower(string &s, const locale &loc)
    {
        for (string::iterator i = s.begin(), j = s.end(); i != j; ++i)
            *i = tolower(*i, loc);
    }

    inline string replaceAll(const string &s, const string &f, const string &r)
    {
        if (s.empty() || f.empty() || f == r || f.size() > s.size() || s.find(f) == string::npos)
        {
            return s;
        }
        ostringstream build_it;
        typedef string::const_iterator iter;
        iter i(s.begin());
        const iter::difference_type f_size(distance(f.begin(), f.end()));
        for (iter pos; (pos = search(i, s.end(), f.begin(), f.end())) != s.end();)
        {
            copy(i, pos, ostreambuf_iterator<char>(build_it));
            copy(r.begin(), r.end(), ostreambuf_iterator<char>(build_it));
            advance(pos, f_size);
            i = pos;
        }
        copy(i, s.end(), ostreambuf_iterator<char>(build_it));
        string out(build_it.str());
        return out;
    }

    int Length;
    vector<string> StopWords;
    vector<string> StopSym;

  public:
    Shingle()
    {
        StopWords.resize(29);
        StopWords[0] = "это";
        StopWords[1] = "как";
        StopWords[2] = "так";
        StopWords[2] = "также";
        StopWords[3] = "и";
        StopWords[4] = "в";
        StopWords[4] = "во";
        StopWords[5] = "над";
        StopWords[6] = "к";
        StopWords[7] = "до";
        StopWords[8] = "не";
        StopWords[9] = "на";
        StopWords[10] = "но";
        StopWords[11] = "за";
        StopWords[12] = "то";
        StopWords[13] = "с";
        StopWords[14] = "ли";
        StopWords[15] = "а";
        StopWords[16] = "во";
        StopWords[17] = "от";
        StopWords[18] = "со";
        StopWords[19] = "для";
        StopWords[20] = "о";
        StopWords[21] = "же";
        StopWords[22] = "ну";
        StopWords[23] = "я";
        StopWords[23] = "ты";
        StopWords[23] = "вы";
        StopWords[24] = "б";
        StopWords[24] = "бы";
        StopWords[25] = "что";
        StopWords[25] = "чтобы";
        StopWords[26] = "кто";
        StopWords[27] = "он";
        StopWords[28] = "она";
        StopWords[28] = "оно";
        StopWords[28] = "они";

        StopSym.push_back(",");
        StopSym.push_back(".");
        StopSym.push_back("!");
        StopSym.push_back("?");
        StopSym.push_back(";");
        StopSym.push_back(":");
        StopSym.push_back("-");
        StopSym.push_back("–");
        StopSym.push_back("+");
        StopSym.push_back("$");
        StopSym.push_back("@");
        StopSym.push_back("#");
        StopSym.push_back("%");
        StopSym.push_back("^");
        StopSym.push_back("&");
        StopSym.push_back("*");
        StopSym.push_back("~");
        StopSym.push_back("'");
        StopSym.push_back("\"");
        StopSym.push_back("№");
        StopSym.push_back("(");
        StopSym.push_back(")");
        StopSym.push_back("\t");
        StopSym.push_back("\r");
        StopSym.push_back("\n");
        StopSym.push_back("  ");
        Length = 10;
    };

    string Canonize(string source)
    {
        string Result = source;
        string_tolower(Result, std::locale(""));

        string res1 = Result;

        for (int i = 0; i < StopSym.size(); i++)
        {
            Result = res1;
            string find_str = "" + StopSym[i];
            res1 = replaceAll(Result, find_str, " ");
        }
        Result.clear();
        Result = res1;

        for (int i = 0; i < StopWords.size(); i++)
        {
            res1 = Result;
            Result = replaceAll(res1, " " + StopWords[i] + " ", " ");
        }

        res1 = Result;
        Result = replaceAll(res1, "  ", " ");

        return Result;
    }

    std::vector<std::string> &split(const std::string &s, char delim, std::vector<std::string> &elems)
    {
        std::stringstream ss(s);
        std::string item;
        while (std::getline(ss, item, delim))
        {
            elems.push_back(item);
        }
        return elems;
    }

    std::vector<std::string> split(const std::string &s, char delim)
    {
        std::vector<std::string> elems;
        split(s, delim, elems);
        return elems;
    }

    CCRC32 crc;

  public:
    void setLength(int l)
    {
        Length = l;
    }

  public:
    int GetShingles(string Text, int *buf)
    {
        int count_shingle = Length;

        vector<string> Words = split(Text, ' ');

#pragma omp parallel for /*private(CurrentShingle)*/
        for (int i = 0; i <= (Words.size() - count_shingle); i++)
        {
            //vector<string> CurrentShingle;

            string ShingleText = "";

            for (int j = 0; j < count_shingle; j++)
            {
                //CurrentShingle.push_back(Words[i + j]);
                ShingleText += (Words[i + j] + " ");
            }
            int res = crc.FullCRC((const unsigned char *)ShingleText.c_str(), (size_t)ShingleText.length());
            buf[i] = res;
        }

        return Words.size() - count_shingle + 1;
    }

  public:
    int CompareShingles(int *bufA, int *bufB, int sizeA, int sizeB)
    {
        //cout << "--- зашли в CompareShingles " << endl;

        int *arrMatches;
        //omp_set_num_threads(1);
        arrMatches = new int[sizeA]; //(int*) _mm_malloc(sizeA * sizeof(int), 0);

        //int arrMatches[16];

        //cout << "--- объявили arrMatches " << endl;

        int matches = 0;

//#pragma omp parallel for reduction (+: matches)
#pragma omp parallel for
        for (int i = 0; i < sizeA; i++)
            arrMatches[i] = 0;

        //cout << "--- обнулили arrMatches " << endl;

#pragma omp parallel for
        for (int i = 0; i < sizeA; i++) // string s in ShingleA)
        {
            //#pragma simd
            for (int j = 0; j < sizeB; j++)
            {
                if (bufB[j] == bufA[i])
                {
                    arrMatches[i]++;
                    break;
                }
            }
        }

        //cout << "--- сравнили, рез-т в массиве arrMatches " << endl;

        matches = 0;
#pragma omp parallel for reduction(+ \
                                   : matches)
        for (int i = 0; i < sizeA; i++)
            matches += arrMatches[i];

        //tot_matches = matches;

        //cout << "--- произвели редукцию с arrMatches " << matches << endl;

        return 2 * 100 * matches / (sizeA + sizeB);
    }

  public:
    int Compare(string TextA, string TextB)
    {
        //cout << "--- зашли в Compare" << endl;

        crc.Initialize();

        int *ShingleA, *ShingleB;

        int sizeAmax = TextA.length();
        int sizeBmax = TextB.length();

        ShingleA = new int[sizeAmax]; //(int*) _mm_malloc(sizeAmax * sizeof(int), 0);
        ShingleB = new int[sizeBmax]; //(int*) _mm_malloc(sizeBmax * sizeof(int), 0);

        //cout << "--- size A = " << sizeAmax << endl;
        //cout << "--- size B = " << sizeBmax << endl;

        int num;
#pragma omp parallel
        {
            num = omp_get_num_threads();
            omp_set_num_threads(num);
        }

        //cout << "--- omp_set_num_threads " << num << endl;

        int sizeA, sizeB;
#pragma omp parallel sections
        {
#pragma omp section
            {
                sizeA = GetShingles(Canonize(TextA), ShingleA);
            }

#pragma omp section
            {
                sizeB = GetShingles(Canonize(TextB), ShingleB);
            }
        }
        //cout << "--- получили шинглы 1 текста: " << sizeA << endl;
        //cout << "--- получили шинглы 2 текста: " << sizeB << endl;

        int res = CompareShingles(ShingleA, ShingleB, sizeA, sizeB);
        //cout << "--- сравнили, результат: " << res << endl;

        return res;
    }

    int openFile(int *buf, const char *path)
    {
        int i = 0;

        FILE *file;
        file = fopen(path, "r");

        while (!feof(file))
        {
            fscanf(file, "%d", &buf[i]);
            i++;
        }
        fclose(file);

        return i;
    }

    void saveFile(int *buf, const char *path, int size)
    {
        FILE *file;
        file = fopen(path, "w");

        for (int i = 0; i < size; i++)
        {
            fprintf(file, "%d\n", buf[i]);
        }
        fclose(file);
    }

  public:
    vector<const char *> getFilesDir(const char *path)
    {
        vector<const char *> files;
        int count = 0;

        glob_t glob_result;
        glob(path, GLOB_TILDE, NULL, &glob_result);

        for (int i = 0; i < glob_result.gl_pathc; ++i)
        {
            files.push_back(glob_result.gl_pathv[i]);
        }

        return files;
    }

  public:
    string cmpBase(const char *text, int sizeText, const char *path)
    {

        crc.Initialize();

        int *textShingle = new int[sizeText];
        int sizeTextShingles = GetShingles(Canonize(text), textShingle);

        vector<const char *> files = getFilesDir(path);

        int count = files.size();
        int res = 0;

        string resStr = "";
        string tmpStr = "";

#pragma omp paralel
        {
            //int num = omp_get_num_threads;
            //omp_set_num_threads(num);
        }

#pragma omp parallel for
        for (int i = 0; i < count; i++)
        {
            int *shingles = new int[sizeText];
            int sizeSh = openFile(shingles, files[i]);

            res = CompareShingles(textShingle, shingles, sizeTextShingles, sizeSh);

            tmpStr += (files[i] + res);

            resStr = resStr + tmpStr;
        }

        return resStr;
    }
};
};
