// See https://aka.ms/new-console-template for more information
using System.Reflection;
using System.IO;

Console.WriteLine("Hello, World!");

//Directory.CreateDirectory(@"C:\tempExp");
//Directory.CreateDirectory(@"C:\tempExp\source");
//Directory.CreateDirectory(@"C:\tempExp\source\primero");
//Directory.CreateDirectory(@"C:\tempExp\source\segundo");
//Directory.CreateDirectory(@"C:\tempExp\source\segundo\primero");
//File.WriteAllText(@"C:\tempExp\source\segundo\primero\abc.txt", "cositas cositas cositas nena");
//File.WriteAllText(@"C:\tempExp\source\segundo\primero\abcd.txt", "cositas cositas cositas nena baby baby");
//File.WriteAllText(@"C:\tempExp\source\segundo\primero\abcde.txt", "Lucia eres un puco puta");
//Directory.CreateDirectory(@"C:\tempExp\source\tercera\segundo");
//Directory.CreateDirectory(@"C:\tempExp\source\tercera\tercera");
//Directory.CreateDirectory(@"C:\tempExp\target");

//List<DirectoryInfo> imageDirs = new List<DirectoryInfo>();

//List<string> dirsSinSubs = Directory.EnumerateDirectories(@"C:\tempExp\source", "*.*", SearchOption.AllDirectories)
//     .Where(f => !Directory.EnumerateDirectories(f, "*.*", SearchOption.TopDirectoryOnly).Any()).ToList();

//foreach (string dir in dirsSinSubs)
//{
//    imageDirs.Add(new DirectoryInfo(dir));
//}



//foreach (DirectoryInfo dir in imageDirs)
//{
//    var rutaVieja = dir.FullName;
//    var rutaNueva = @"C:\tempExp\target\" + dir.Name;
//    try
//    {
//        copiarSubdirectorio(dir, rutaNueva);
//    }
//    catch (IOException ex)
//    {
//        int i = 0;
//        while (Directory.Exists(rutaNueva))
//        {
//            i++;
//            rutaNueva = @"C:\tempExp\target\" + dir.Name + $"_{i}";
//        }
//        copiarSubdirectorio(dir, rutaNueva);
//    }
//    catch (Exception ex) { Console.WriteLine(ex); }
//    finally
//    {
//        File.WriteAllText(rutaNueva + "\\origin.txt", rutaVieja);
//    }
//}

//Z:\family_media\Prueba
//Z:\importBay\D6\Prueba

//DirectoryInfo origin = new DirectoryInfo(@"Z:\importBay\D6\Prueba");


void copiarSubdirectorio(DirectoryInfo di, string rutaDestino)
{
    if (!Directory.Exists(rutaDestino))
        Directory.CreateDirectory(rutaDestino);
    else
        throw new IOException("Ya existe el directorio, prueba otro nombre");

    foreach (FileInfo fi in di.GetFiles())
    {
        if (fi.Name != "thumbs.db")
            fi.MoveTo(Path.Combine(rutaDestino, fi.Name));
    }
}

//try
//{

//    copiarSubdirectorio(origin, @"Z:\family_media\Prueba");
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
//Console.ReadKey();


string rutaBase = @"Z:\importBay\";
string rutaDestino = @"Z:\family_media\";
string cacheDirectory = "ca.che";
List<string> Ddirs = Directory.GetDirectories(rutaBase, "D*", SearchOption.TopDirectoryOnly).ToList();


List<DirectoryInfo> imageDirs = new List<DirectoryInfo>();

imageDirs = CacheDirs(Ddirs, cacheDirectory);

int j = 0;
foreach (DirectoryInfo dir in imageDirs)
{
    Console.WriteLine($"Copiando directorio {j}/{imageDirs.Count}: {dir.Name}");
    var rutaVieja = dir.FullName;
    var rutaNueva = rutaDestino + dir.Name;
    try
    {
        copiarSubdirectorio(dir, rutaNueva);
    }
    catch (IOException ex)
    {
        int i = 0;
        while (Directory.Exists(rutaNueva))
        {
            i++;
            rutaNueva = rutaDestino + dir.Name + $"_{i}";
        }
        copiarSubdirectorio(dir, rutaNueva);
    }
    catch (Exception ex) { Console.WriteLine(ex); }
    finally
    {
        File.WriteAllText(rutaNueva + "\\TNS_DISK_origin.txt", rutaVieja);
    }
    j++;
}


static List<DirectoryInfo> CacheDirs(List<string> Ddirs, string cacheFile)
{
    List<DirectoryInfo> imageDirs = new List<DirectoryInfo>();
    List<string> dirsSinSubs = new List<string>();

    if (File.Exists(cacheFile))
    {
        var listaCache = File.ReadAllLines(cacheFile).ToList();
        listaCache.ForEach(d => imageDirs.Add(new DirectoryInfo(d)));
        return imageDirs;
    }

    //Buscamos todos los directorios hoja que tengan contenido (el que sea)
    foreach (string dir in Ddirs)
    {
        dirsSinSubs.AddRange(Directory.EnumerateDirectories(dir, "*.*", SearchOption.AllDirectories)
             .Where(f => !Directory.EnumerateDirectories(f, "*.*", SearchOption.TopDirectoryOnly).Any()).ToList());
    }

    //Filtramos los directorios que tienen estas extensiones, al menos 3
    foreach (string dir in dirsSinSubs)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        List<FileInfo> files =
        [
            .. dirInfo.GetFiles("*.jpg"),
            .. dirInfo.GetFiles("*.jpeg"),
            .. dirInfo.GetFiles("*.png"),
        ];
        if (files.Count > 3)
        {
            imageDirs.Add(dirInfo);
        }
        Console.WriteLine(dir);
    }

    List<string> stringDirs = new List<string>();
    imageDirs.ForEach(d => stringDirs.Add(d.FullName));
    File.WriteAllLines(cacheFile, stringDirs);

    return imageDirs;
}