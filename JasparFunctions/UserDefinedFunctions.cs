using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    private static string ReverseRaw(string seq)
    {
        seq = seq.ToUpper();

        // Make sure it only containts A, T, C and G
        Regex rgx = new Regex("[^ATCG]");
        seq = rgx.Replace(seq, "");

        char[] chr = seq.ToCharArray();
        int Length = seq.Length;

        for (int i = 0; i < seq.Length; i++)
        {
            switch (seq[i])
            {
                case 'A':
                    chr[Length - i - 1] = 'T';
                    break;
                case 'T':
                    chr[Length - i - 1] = 'A';
                    break;
                case 'C':
                    chr[Length - i - 1] = 'G';
                    break;
                case 'G':
                    chr[Length - i - 1] = 'C';
                    break;
            }
        }

        return new string(chr);
    }
        
    private class JASPARResult
    {
        public int _Position = 0;
        public int _Strand = 1;
        public double _Score = 0;
        public double _RelativeScore = 0;
        public string _Sequence = "";

        public JASPARResult(int Position, int Strand, double Score, double RelativeScore, string Sequence)
        {
            _Position = Position;
            _Strand = Strand;
            _Score = Score;
            _RelativeScore = RelativeScore;
            _Sequence = Sequence;
        }
    }

    private class JASPARVariantResult
    {
        public int _Position = 0;
        public int _Strand = 1;
        public double _RefScore = 0;
        public double _AltScore = 0;

        public JASPARVariantResult(int Position, int Strand, double RefScore, double AltScore)
        {
            _Position = Position;
            _Strand = Strand;
            _RefScore = RefScore;
            _AltScore = AltScore;
        }
    }

    static private int[,] JASPARGetPFM(string tf)
    {
        int[,] PFM = null;
        if (tf.ToUpper() == "MA0035.3" || tf.ToUpper() == "GATA1") // MA0035.3 (Mus musculus)
        {
            PFM = new int[,] {
                {  2035,  4321,  2530,  9069},
                {   506,  6406,  2792,  8251},
                {     0, 14869,   437,  2649},
                {     0,   697,     0, 17258},
                {     0,     0,     0, 17955},
                { 17955,     0,     0,     0},
                {     0,     0,     0, 17955},
                {     0, 17955,     0,     0},
                {  2166,     0,     0, 15789},
                {  2502,  6166,  6687,  2600},
                {  1886,  5072,  1598,  9399}
            };
        }
        else if (tf.ToUpper() == "MA0035.4") // MA0035.4 (Homo sapiens) (GATA1)
        {
            PFM = new int[,] {
                { 22209, 12209, 13955, 23455},
                { 17328, 14489, 11088, 28923},
                {  3953, 62419,  2712,  2744},
                {  1314,   595,   652, 69267},
                { 49692,   710,   856, 20570},
                { 67550,  1292,   988,  1998},
                {  2206,  1238,   618, 67766},
                {  2567, 65937,  1471,  1853},
                {  7397,  3025,   765, 60641},
                { 26545, 11186, 14358, 19739},
                { 22656, 15261,  9325, 24586}
            };
        }
        else if (tf.ToUpper() == "MA0493.1" || tf.ToUpper() == "KLF1") // MA0493.1 (Mus musculus)
        {
            PFM = new int[,] {
                { 150,  76, 202,  98},
                { 167,  40, 305,  14},
                {  67, 396,  32,  31},
                {  13, 461,   0,  52},
                { 404, 118,   0,   4},
                {   0, 526,   0,   0},
                { 400,   0,  97,  29},
                {   0, 526,   0,   0},
                {   0, 526,   0,   0},
                {   0, 508,   0,  18},
                { 280,  35,   0, 211}
            };
        }
        else if (tf.ToUpper() == "MA0002.1" || tf.ToUpper() == "RUNX1") // MA0002.1 (Homo sapiens)
        {
            PFM = new int[,]
            {
                { 10,  2,  3, 11},
                { 12,  2,  1, 11},
                {  4,  7,  1, 14},
                {  1,  1,  0, 24},
                {  2,  0, 23,  1},
                {  2,  8,  0, 16},
                {  0,  0, 26,  0},
                {  0,  0, 26,  0},
                {  0,  1,  0, 25},
                {  8,  2,  0, 16},
                { 13,  2,  4,  7}
            };
        }
        else if (tf.ToUpper() == "MA0002.2") // MA0002.2 (Mus musculus) (RUNX1)
        {
            PFM = new int[,]
            {
                { 287,  496,  696,  521},
                { 234,  485,  467,  814},
                { 123, 1072,  149,  656},
                {  57,    0,    7, 1936},
                {   0,   75, 1872,   53},
                {  87,  127,   70, 1716},
                {   0,    0, 1987,   13},
                {  17,   42, 1848,   93},
                {  10,  400,  251, 1339},
                { 131,  463,   81, 1325},
                { 500,  158,  289, 1053}
            };
        }
        else if (tf.ToUpper() == "MA0750.2" || tf.ToUpper() == "LRF") // MA0750.2 (Homo sapiens)
        {
            PFM = new int[,]
            {
                {  4136,  3512,  4167,  2826},
                {  2783,  5356,  4397,  2105},
                {   477, 12469,  1496,   199},
                {  2490, 11293,   624,   234},
                {   173,   210, 14130,   128},
                {   114,   163, 14239,   125},
                { 13854,   274,   335,   178},
                { 13719,   343,   261,   318},
                {   629,   711, 13112,   189},
                {  1141,  3433,  1623,  8444},
                {  1744,  2485,  9142,  1270},
                {  2709,  4386,  5862,  1684},
                {  2850,  4856,  4669,  2266}
            };
        }
        else
            throw new Exception("Invalid transcription factor!");

        return PFM;
    }

    static private double[,] JASPARGetPPM(string tf)
    {
        // Background distribution for A, C, G and T
        double[] bgDist = new double[] { 0.25, 0.25, 0.25, 0.25 };

        int[,] PFM = JASPARGetPFM(tf);
        int len = PFM.GetLength(0);

        double[,] PPM;
        PPM = new double[len, 4];

        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int total = PFM[i, 0] + PFM[i, 1] + PFM[i, 2] + PFM[i, 3];
                double sqrTotal = Math.Sqrt(total);
                PPM[i, j] = (PFM[i, j] + sqrTotal * bgDist[j]) / (total + sqrTotal);
            }
        }

        return PPM;
    }

    static private double[,] JASPARGetPWM(string tf, out double min, out double max)
    {
        // Background distribution for A, C, G and T
        double[] bgDist = new double[] { 0.25, 0.25, 0.25, 0.25 };

        min = 0;
        max = 0; 

        double[,] PPM = JASPARGetPPM(tf);
        int len = PPM.GetLength(0);

        double[,] PWM;
        PWM = new double[len, 4];
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                PWM[i, j] = Math.Log(PPM[i, j] / bgDist[j], 2);
            }
        }
        
        for (int i = 0; i < len; i++)
        {
            max += Max(PWM[i, 0], PWM[i, 1], PWM[i, 2], PWM[i, 3]);
            min += Min(PWM[i, 0], PWM[i, 1], PWM[i, 2], PWM[i, 3]);
        }

        return PWM;
    }

    [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "JASPARScanSequenceFillRow", TableDefinition = "Position int, Strand int, Score float, RelativeScore float, Sequence nvarchar(32)")]
    public static IEnumerable JASPARScanSequence(SqlString SqlTranscriptionFactor, SqlDouble SqlThreshold, SqlString SqlSequence)
    {
        string tf = SqlTranscriptionFactor.ToString();
        double threshold = (double)SqlThreshold;
        string seq = SqlSequence.ToString();

        // Background distribution for A, C, G and T
        double[] bgDist = new double[] { 0.25, 0.25, 0.25, 0.25 };

        int[,] PFM = JASPARGetPFM(tf);
        int len = PFM.GetLength(0);

        double[,] PPM;
        PPM = new double[len, 4];

        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int total = PFM[i, 0] + PFM[i, 1] + PFM[i, 2] + PFM[i, 3];
                double sqrTotal = Math.Sqrt(total);
                PPM[i, j] = (PFM[i, j] + sqrTotal * bgDist[j]) / (total + sqrTotal);
            }
        }

        double[,] PWM;
        PWM = new double[len, 4];
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                PWM[i, j] = Math.Log(PPM[i, j] / bgDist[j], 2);
            }
        }

        double max = 0, min = 0;
        for (int i = 0; i < len; i++)
        {
            max += Max(PWM[i, 0], PWM[i, 1], PWM[i, 2], PWM[i, 3]);
            min += Min(PWM[i, 0], PWM[i, 1], PWM[i, 2], PWM[i, 3]);
        }

        List<JASPARResult> resultList = new List<JASPARResult>();
        for (int s = 0; s + len <= seq.Length; s++)
        {
            string scanseqf = seq.Substring(s, len);
            string scanseqr = ReverseRaw(scanseqf);

            double scoref = 0, scorer = 0;
            for (int i = 0; i < len; i++)
            {
                scoref += PWM[i, GetNucleotideIndex(scanseqf[i])];
                scorer += PWM[i, GetNucleotideIndex(scanseqr[i])];
            }

            double relscoref = (scoref - min) / (max - min);
            double relscorer = (scorer - min) / (max - min);


            if (relscoref >= threshold)
                resultList.Add(new JASPARResult(s + 1, 1, scoref, relscoref, scanseqf));

            if (relscorer >= threshold)
                resultList.Add(new JASPARResult(s + 1, -1, scorer, relscorer, scanseqf));
        }

        return resultList.ToArray();
    }

    private static void JASPARScanSequenceFillRow(Object obj, out SqlInt32 Position, out SqlInt32 Strand, out SqlDouble Score, out SqlDouble RelativeScore, out SqlString Sequence)
    {
        JASPARResult res = (JASPARResult)obj;
        Position = res._Position;
        Strand = res._Strand;
        Score = res._Score;
        RelativeScore = res._RelativeScore;
        Sequence = res._Sequence;
    }

    [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "JASPARScanVariantFillRow", TableDefinition = "Position int, Strand int, RefScore float, AltScore float")]
    public static IEnumerable JASPARScanVariant(SqlString SqlTranscriptionFactor, SqlDouble SqlThreshold, SqlString SqlRefSequence, SqlString SqlAltSequence)
    {
        string tf = SqlTranscriptionFactor.ToString();
        double threshold = (double)SqlThreshold;
        string refSequence = SqlRefSequence.ToString();
        string altSequence = SqlAltSequence.ToString();

        double min = 0, max = 0;
        double[,] PWM = JASPARGetPWM(tf, out min, out max);
        int len = PWM.GetLength(0);

        List<JASPARVariantResult> resultList = new List<JASPARVariantResult>();
        for (int s = 0; s + len <= refSequence.Length && s + len <= altSequence.Length; s++)
        {
            string refseqf = refSequence.Substring(s, len);
            string refseqr = ReverseRaw(refseqf);
            string altseqf = altSequence.Substring(s, len);
            string altseqr = ReverseRaw(altseqf);

            double scorereff = 0, scorerefr = 0, scorealtf = 0, scorealtr = 0;
            for (int i = 0; i < len; i++)
            {
                scorereff += PWM[i, GetNucleotideIndex(refseqf[i])];
                scorerefr += PWM[i, GetNucleotideIndex(refseqr[i])];
                scorealtf += PWM[i, GetNucleotideIndex(altseqf[i])];
                scorealtr += PWM[i, GetNucleotideIndex(altseqr[i])];
            }

            double relscorereff = (scorereff - min) / (max - min);
            double relscorerefr = (scorerefr - min) / (max - min);
            double relscorealtf = (scorealtf - min) / (max - min);
            double relscorealtr = (scorealtr - min) / (max - min);


            if (relscorereff >= threshold || relscorealtf >= threshold)
                resultList.Add(new JASPARVariantResult(s + 1, 1, relscorereff, relscorealtf));

            if (relscorerefr >= threshold || relscorealtr >= threshold)
                resultList.Add(new JASPARVariantResult(s + 1, -1, relscorerefr, relscorealtr));
        }

        return resultList.ToArray();
    }

    private static void JASPARScanVariantFillRow(Object obj, out SqlInt32 Position, out SqlInt32 Strand, out SqlDouble RefScore, out SqlDouble AltScore)
    {
        JASPARVariantResult res = (JASPARVariantResult)obj;
        Position = res._Position;
        Strand = res._Strand;
        RefScore = res._RefScore;
        AltScore = res._AltScore;
    }

    private static double Max(double a, double b, double c, double d = Double.MinValue)
    {
        return Math.Max(Math.Max(Math.Max(a, b), c), d);
    }

    private static double Min(double a, double b, double c, double d)
    {
        return Math.Min(Math.Min(Math.Min(a, b), c), d);
    }

    private static int GetNucleotideIndex(char n)
    {
        switch (n)
        {
            case 'A': return 0;
            case 'C': return 1;
            case 'G': return 2;
            case 'T': return 3;
        }

        return 0;
    }
}