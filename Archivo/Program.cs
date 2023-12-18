using System.IO;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;
using System.Threading;

void Main()
{
    //LINEA QUE CREA EL FICHERO Y CONTIENE LAS LINEAS
    string[] lineas = File.ReadAllLines("C:\\Users\\adrix\\RiderProjects\\Archivo\\Archivo\\password.txt");
    //Generar random
    int random = new Random().Next(1,lineas.Length);
    //Seleccionar palabta y encriptarla
    string palabra = Encrypt.GetSHA256(lineas[random]);
    //Booleano para que termine
    bool terminarproceso = false;
    //Imprime la palabra a buscar
    Console.WriteLine($"la palabra a encontrar cifrada es {palabra} ");

    //Instancia de la 
    LectorHilos programahilos = new LectorHilos();
    
    programahilos.fragmentareiniciar(palabra,lineas,5);
}

Main();

public class LectorHilos
{
    //Variable que, al encontrar la palabra buscada, se pondra a true y se detendrá la ejecucion
    private bool encontrado;
    
    //Lista de hilos
    
    List<Thread> lista = new List<Thread>();
    
    //Funcion que recibe la palabra cifrada, las lineas (el archivo), y el numero de hilos por el que se desea analizar el archivo
   public void fragmentareiniciar(string palabracrypted,string[] lineas,int numerohilos)
    {
        //Dividir las lineas entre el numero de hilos
        
        var fragmentolista = lineas.Length / numerohilos;
        
        /*Bucle por cada numero de hilos, añadir a la lista un nuevo hilo que contendra la funcion recorrer , y en cada iteraccion
         el numero de comienzo sera el final de la antigua interaccion 
         */
        
        for (int x = 0; x < numerohilos; x++)
        {
            var xmultiplicable = x;
            lista.Add(new Thread(() => recorrer(palabracrypted,lineas,fragmentolista * xmultiplicable, Math.Min(fragmentolista * (xmultiplicable + 1),lineas.Length))));
        }
        
        //Inicia los hilos
        foreach (var hilo in lista)
        {
            hilo.Start();    
        }
        
    }
    
   //Funcion recorrer que recibe la palabra encriptada, las lineas, y el inicio y fin de el bucle
    void recorrer(string palabracrypted,string[] lineas,int inicio, int fin)
    {
        for (int x = inicio; x < fin; x++)
        {
            //Si ya se ha encontrado, no se ha de seguir buscando
            if (encontrado)
            {
                break;
            }
            //Si la linea actual (encriptada) es igual a la que se busca, atributo encontrado se pone true
            if (palabracrypted==Encrypt.GetSHA256(lineas[x]))
            {
                Console.WriteLine($"ENCONTRADA, SU PALABRA ES: \n DESENCRIPTADA -> {lineas[x]} \n ENCRIPTADA {Encrypt.GetSHA256(lineas[x])} ");
                encontrado = true;
            }
        }
    }
}
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

