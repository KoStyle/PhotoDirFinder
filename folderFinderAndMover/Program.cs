// See https://aka.ms/new-console-template for more information
using System.Reflection;

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
List<string> Ddirs = Directory.GetDirectories(rutaBase, "D*", SearchOption.TopDirectoryOnly).ToList();


List<DirectoryInfo> imageDirs = new List<DirectoryInfo>();

List<string> dirsSinSubs = Directory.EnumerateDirectories(Ddirs[5], "*.*", SearchOption.AllDirectories)
     .Where(f => !Directory.EnumerateDirectories(f, "*.*", SearchOption.TopDirectoryOnly).Any()).ToList();

foreach (string dir in dirsSinSubs)
{
    DirectoryInfo dirInfo = new DirectoryInfo(dir);
    List<FileInfo> files =
    [
        .. dirInfo.GetFiles("*.jpg"),
        .. dirInfo.GetFiles("*.jpeg"),
        .. dirInfo.GetFiles("*.png"),
    ];
    if (files.Count > 5)
    {
        imageDirs.Add(dirInfo);
    }
    Console.WriteLine(dir);
}

using (StreamWriter outputFile = new StreamWriter("dirImagenes"))
{
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

}
