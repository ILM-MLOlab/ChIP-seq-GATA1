// Data sourcebase and path to project directory
var source = "localhost";
var path = path_to_project_directory;

// Database name
var db = "JasparFunctions";

var WshShell = new ActiveXObject("WScript.Shell");
var dir = WScript.Echo(WshShell.CurrentDirectory);

var fso = new ActiveXObject("Scripting.FileSystemObject");
var textFile = fso.OpenTextFile(path + "\\Scripts\\DeployTemplate.sql", 1, 2);
var sqlFirst = textFile.ReadAll(); 
var sqlLast = "";
textFile.Close();

var found = false;
textFile = fso.OpenTextFile(path + "\\obj\\debug\\JasparFunctions.generated.sql", 1, 2);
while (!textFile.AtEndOfStream) {
    var line = textFile.ReadLine();

    if (line.indexOf("--") == -1) {
        if (!found)
            found = line.search("CREATE ASSEMBLY") > -1;

        if (found)
            sqlFirst += line + "\r\n";
        else
            sqlLast += line + "\r\n";
    }
}
textFile.Close();

var sql = sqlFirst + sqlLast;

textFile = fso.CreateTextFile(path + "\\SQL\\DeployCLR.sql", true);
textFile.Write(sql);
textFile.Close();

var sqlArray = sql.split("GO\r\n");

var connectionString = "Provider=MSOLEDBSQL;Integrated Security=SSPI;Initial Catalog=" + db + ";Data Source=" + source;
var conn = new ActiveXObject("ADODB.Connection");
conn.Open(connectionString);

var cmd = new ActiveXObject("ADODB.Command");
cmd.CommandType = 1;
cmd.ActiveConnection = conn;

for (var i = 0; i < sqlArray.length; i++) {
    cmd.CommandText = sqlArray[i];
    cmd.Execute();
}

conn.Close();