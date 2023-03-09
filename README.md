# ChIP-seq-GATA1
Here is the depository for the code and source data for ChIP-seq GATA1 manuscript

The workflow of the analysis pipeline
![image](https://user-images.githubusercontent.com/102995282/224078370-2fc1b42f-86ea-45ea-b3ce-d592e298b951.png)

* number of overlapping peaks in the descending order of number of datasets. 193 sites were found to to be overlapping from the reference dataset with at least one other dataset, 156 sites were found to be overlapping from the reference dataset with at least another two datasets, 114 sites were found to be found in all four datasets.

## Description
This project aims to find the GATA1 binding motifs in the blood group genes, utilizing publicly available datasets of GATA1 ChIP-seq on primary adult erythroid cells, to collectively generate GATA1 binding sites that are in agreement from different datasets to predict the sites that can affect blood group gene expression. 

### Platforms
* Computing and storage on high-performance computer (HPC) associated with Lund University (LUNARC)
* OpenRefine, version 3.5.2
* Rstudio, version 3.6.3

## Executing scripts in transition between platforms
1. [HPC_1](https://github.com/ILM-MLOlab/ChIP-seq-GATA1/blob/main/HPC/HPC_1) (from blue to purple and to part of orange section in the figure)
* Download the public dataset directly to HPC, and individually process through nf-core ChIP-seq analysis pipeline, merge BAM files for the replicate datasets and, intersect all narrow peaks, get FASTA file on the merged bed file with hg38, and find GATA1 motif with FIMO from MEME suite.
2. [OpenRefine_1](https://github.com/ILM-MLOlab/ChIP-seq-GATA1/blob/main/OpenRefine/OpenRefine_1) (part of the orange section in the figure)
* To sort the FIMO files in proper order to be combined togeher with the JASPAR track from the [JASPAR function](https://github.com/ILM-MLOlab/ChIP-seq-GATA1/tree/main/JasparFunctions)
3. [HPC_2](https://github.com/ILM-MLOlab/ChIP-seq-GATA1/blob/main/HPC/HPC_2) (part of the orange section in the figure)
* Change the file format, from tsv to bed file and get rid of the header to join FIMO and JASPAR files
4. [OpenRefine_2](https://github.com/ILM-MLOlab/ChIP-seq-GATA1/blob/main/OpenRefine/OpenRefine_2) (part of the orange section in the figure)
* Clean up the joined FIMO-JASPAR files by renaming, deleting and rearragning columns
5. [HPC_3](https://github.com/ILM-MLOlab/ChIP-seq-GATA1/blob/main/HPC/HPC_3) (part of the purple section in the figure)
* Clean up the data to remove the header 
6. [R script](https://github.com/ILM-MLOlab/ChIP-seq-GATA1/blob/main/R%20script/R%20script) (from purple to orange and then yellow section in the figure)
* Annotate the peaks and join together with the FIMO-JASPAR track, and filter down to blood group genes
7. [OpenRefine_3](https://github.com/ILM-MLOlab/ChIP-seq-GATA1/blob/main/OpenRefine/OpenRefine_3) (yellow section in the figure)
* Clean up the annotated file to remove and rename fields appropriately
8. After step 7, there will be 359 rows, then we further filtered it down by in OpenRefine to facet the column "SYMBOL" and exclude the transcription factor genes "GATA1" and "KLF1", and facet the column "overlap" to exclude the value of "0". Now, there should be 193 rows that are found only in blood group genes and have at least 2 data sets having peaks overlapping at the site of the motif.

## Author
Ping Chun Wu
