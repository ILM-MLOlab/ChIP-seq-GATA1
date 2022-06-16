# load files
Intersect_with_Cis_3_GATA1_adult_sort.bed <- read.delim("~/Documents/LU/ENCODE/ENCODE/analysis2022/GATA1/intersect/OpenRefine/Intersect_with_Cis_3_GATA1_adult_sort.bed.tsv")

# load packages
library(rtracklayer)
library(Rsamtools)
library(BiocManager)
BiocManager::install("TxDb.Hsapiens.UCSC.hg38.knownGene")
txdb <- TxDb.Hsapiens.UCSC.hg38.knownGene::TxDb.Hsapiens.UCSC.hg38.knownGene
library(ChIPpeakAnno)
library(ChIPseeker)
library(tidyverse)
library(dplyr)


# try to annotate the file after overlap
Intersect_Cis_3_GATA1_adult_sort_peak <- readPeakFile("Intersect_with_Cis_3_GATA1_adult_sort.bed")
Intersect_Cis_3_GATA1_adult_sort_peak_annotate <- annotatePeak(Intersect_Cis_3_GATA1_adult_sort_peak, tssRegion = c(-5000, 5000), TxDb = txdb, annoDb = "org.Hs.eg.db")
Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df <- data.frame(Intersect_Cis_3_GATA1_adult_sort_peak_annotate)
View(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df)


# name the columns
names(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df)[1] <-'Chr'
names(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df)[6] <-'sequence_name'
names(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df)[7] <-'peak_score'
names(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df)[8] <-'signal_value'
names(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df)[9] <-'pValue'
names(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df)[10] <-'qValue'
names(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df)[11] <-'peak_summit'
names(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df)[12] <-'overlap'

write.csv(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df, file="Intersect_Cis_3_GATA1_adult_sort_peak_annotate.csv")


# load the file from FIMO_JASPAR
# innerjoin by sequenc_name
Intersect_Cis_3_GATA1_FIMMO_JASPAR_annotate <- inner_join(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df,Intersect_with_Cis_3_GATA1_FIMO_JASPAR_sort, by ='sequence_name')
View(Intersect_Cis_3_GATA1_adult_sort_peak_annotate_df)
View(Intersect_with_Cis_3_GATA1_FIMO_JASPAR_sort)
View(Intersect_Cis_3_GATA1_FIMMO_JASPAR_annotate)  

# load gene symbol file
bloodgroupgenesymbol <- read.csv("~/Documents/LU/ENCODE/ENCODE/bloodgroupgenesymbol.csv", sep="")
View(bloodgroupgenesymbol)

# semijoin with blood group genes
Intersect_Cis_3_GATA1_FIMO_JASPAR_annotate_blood <- semi_join(Intersect_Cis_3_GATA1_FIMMO_JASPAR_annotate, bloodgroupgenesymbol, by = 'SYMBOL')
View(Intersect_Cis_3_GATA1_FIMO_JASPAR_annotate_blood)  

write.table(Intersect_Cis_3_GATA1_FIMO_JASPAR_annotate_blood, file ="Intersect_Cis_3_GATA1_FIMO_JASPAR_annotate_blood.tsv", sep = '\t', row.names = FALSE, col.names = TRUE)

count(Intersect_Cis_3_GATA1_FIMO_JASPAR_annotate_blood, overlap >='3')
count(Intersect_Cis_3_GATA1_FIMO_JASPAR_annotate_blood, overlap ='2')
count(Intersect_Cis_3_GATA1_FIMO_JASPAR_annotate_blood, overlap)
count(Intersect_Cis_3_GATA1_FIMO_JASPAR_annotate_blood, diff_FIMO_Jaspar)
