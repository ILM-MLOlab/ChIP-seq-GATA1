# JASPAR Functions

## Description

A CLR project for Microsoft SQL Server for predicting transcription factor binding sites. Extracted from the Erythrogene project<sup>1</sup>.

## Getting Started

### Dependencies

* Requires Microsoft SQL Server.
* Built and tested on Microsoft SQL Server 2017 Developer (64-bit), Version 14.0.2042.3 on Windows 10 Pro 21H2.
* CLR (Common Language Runtime) functions were built with Microsoft Visual Studio 2017.

### Installing

* Create a new database called "JasparFunctions".
* Configure the new database to accept CLR procedures, see https://docs.microsoft.com/en-us/sql/relational-databases/clr-integration/clr-integration-enabling?view=sql-server-2017 for details.
* Set source (SQL Server instance name) and project path in postbuild.js.
* Build project JasparFunctions.proj.

### Executing program

* Start Microsoft SQL Server Management Studio.
* Activate database "JasparFunctions".
* Run the following code:
* 
```
SELECT * FROM dbo.JASPARScanSequence('GATA1', '0.80' , 'ACATCTGCAAAGATCTTATCTCCAAAGAAA')
```

This code will list all predicted GATA1 binding sites in the sequence "ACATCTGCAAAGATCTTATCTCCAAAGAAA" with a RelativeScore > 0.80 and produce the output below (one predicted GATA1 binding site found). The first parameter is the transcription factor ('GATA1', 'KLF1', 'LRF' or 'RUNX1'). The second parameter is the lower threshold for the relative score. The third parameter is the sequence to be searched.

Output:
| Name            | Value             |
| --------------- | ----------------- |
| Position        | 13                |
| Strand          | 1                 |
| Score           | 13.8051480784118  |
| RelativeScore   | 0.956981023301481 |
| Sequence        | ATCTTATCTCC       |

Position: The position in the submitted sequence where the predicted TFBS is located.<br>
Strand: The strand where the TFBS is found. 1 = positive strand. 2 = negative strand.<br>
Score: The absolute score.<br>
Relative Score: The relative score.<br>
Sequence: The sequence matched.<br>

## Author

Mattias Möller

## Version History

* 1.0
    * First release uploaded to GitHub

## References

<sup>1</sup> Mattias Möller, Magnus Jöud, Jill R. Storry, Martin L. Olsson; Erythrogene: a database for in-depth analysis of the extensive variation in 36 blood group systems in the 1000 Genomes Project. *Blood Adv* 2016; 1 (3): 240–249. doi: https://doi.org/10.1182/bloodadvances.2016001867