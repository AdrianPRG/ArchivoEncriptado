using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Threading;


//LINEA QUE CREA EL FICHERO Y CONTIENE LAS LINEAS
string[] lineas = File.ReadAllLines("C:\\Users\\adrix\\RiderProjects\\Archivo\\Archivo\\password.txt");
//GENERAR RANDOM
Random random = new Random();
//ASIGNA RANDOM
int numero = random.Next(1, lineas.Length);
//SELECICONA PALABRA RANDOM
string palabra = lineas[numero];
//LA ENCRIPTA
string encriptada = Encrypt.GetSHA256(palabra);
//booleano para que termine
bool terminarproceso = false;
//IMPRIME
Console.WriteLine("la palabra a encontrar cifrada es ");
Console.WriteLine(encriptada);


//FUNCION PARA RECORRER LA PRIMERA MITAD DEL ARCHIVO

void encriptar1()
{
    for(int x = 0; x < lineas.Length/2; x++)
    {
        string lineaactual = Encrypt.GetSHA256(lineas[x]);
        if (terminarproceso)
        {
            break;
        }
        {
            if (lineaactual == encriptada)
            {
                Console.WriteLine("LA PALABRA ENCONTRADA QUE ES IGUAL A CIFRADA ES: ");
                Console.WriteLine(lineas[x]);
                terminarproceso = true;
            }    
        }
    }    
}

//FUNCION PARA RECORRER LA SEGUNDA MITAD DEL ARCHIVO


void encriptar2()
{
    for(int x = lineas.Length/2; x < lineas.Length; x++)
    {
        string lineaactual = Encrypt.GetSHA256(lineas[x]);
        if (terminarproceso)
        {
            break;
        }
        {
            if (lineaactual == encriptada)
            {
                Console.WriteLine("LA PALABRA ENCONTRADA QUE ES IGUAL A CIFRADA ES: ");
                Console.WriteLine(lineas[x]);
                terminarproceso = true;
            }    
        }
    }    
}

void Main()
{
    Thread hilo1 = new Thread(encriptar1);
    Thread hilo2 = new Thread(encriptar2);
    
    hilo1.Start();
    hilo2.Start();

}

Main();

public class Encrypt{
    public static string GetSHA256(string str)
    {
        SHA256 sha256 = SHA256Managed.Create();
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] stream = null;
        StringBuilder sb = new StringBuilder();
        stream = sha256.ComputeHash(encoding.GetBytes(str));
        for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
        return sb.ToString();
    }
 
}

