using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public InputField nombre;
    public InputField apellido;
    public InputField anio;
    public InputField cedula;
    public Text mensaje;
    public Button delete;
    public Button update;
    public GameObject content;
    public GameObject listaPrefab;
    int idGeneral;
    // Start is called before the first frame update
    void Start()
    {
        borrarDatos();
    }
    public void Listar()
    {
        while (content.transform.childCount>0)
        {
            Transform c = content.transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
        bool consulto = false;
        string conn = "URI=file:" + Application.dataPath + "/Plugins/Datos.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM Persona";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            consulto = true;
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string apellidoC = reader.GetString(2);
            string anioC = reader.GetString(3);
            string cedulaC = reader.GetString(4);

            GameObject go = (GameObject)Instantiate(listaPrefab);
            go.transform.SetParent(content.transform);
            go.transform.Find("idlb").GetComponent<Text>().text = id.ToString();
            go.transform.Find("nombrelb").GetComponent<Text>().text = name;
            go.transform.Find("apellidolb").GetComponent<Text>().text = apellidoC;
            go.transform.Find("aniolb").GetComponent<Text>().text = anioC;
            go.transform.Find("cedulalb").GetComponent<Text>().text = cedulaC;

        }
        if (!consulto)
        {
            mensaje.GetComponent<Text>().text = "****************** No Hay DATOS *********************";
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        for (int i = 0; i < 5; i++)
        {
            
        }
    }
    public void Actualizar()
    {
        string conn = "URI=file:" + Application.dataPath + "/Plugins/Datos.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "UPDATE Persona SET nombre='"+nombre.text+"',apellido='"+apellido.text+"',anionacimiento='"+anio.text+"',cedula='"+cedula.text+"' Where id="+idGeneral;
        Debug.Log(sqlQuery);
        dbcmd.CommandText = sqlQuery;

        if (dbcmd.ExecuteNonQuery() == 1)
        {
            mensaje.GetComponent<Text>().text = "****************** Actualizado Correctamente *********************";
            
        }
        else
        {
            mensaje.GetComponent<Text>().text = "****************** Fallo al Actualizar *********************";
            

        }
        borrarDatos();
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    public void Eliminar()
    {
        string conn = "URI=file:" + Application.dataPath + "/Plugins/Datos.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "DELETE FROM Persona Where id ='"+idGeneral+"'";
        dbcmd.CommandText = sqlQuery;

        if (dbcmd.ExecuteNonQuery() == 1)
        {
            mensaje.GetComponent<Text>().text = "****************** Borrado Correctamente *********************";
            
        }
        else
        {
            mensaje.GetComponent<Text>().text = "****************** Fallo al Borrar *********************";
            
        }
        borrarDatos();
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    public void borrarDatos()
    {
        nombre.text = "";
        apellido.text = "";
        anio.text = "";
        cedula.text = "";
        idGeneral = 0;
        delete.interactable = false;
        update.interactable = false;
    }
    public void Guardar()
    {
        string conn = "URI=file:" + Application.dataPath + "/Plugins/Datos.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "INSERT INTO Persona (nombre,apellido,anionacimiento,cedula) VALUES ('"+nombre.text+"','"+ apellido.text + "','" + anio.text + "','" + cedula.text + "')";
        dbcmd.CommandText = sqlQuery;
        
        if (dbcmd.ExecuteNonQuery()==1)
        {
            mensaje.GetComponent<Text>().text = "****************** Guardado Correctamente *********************";
            borrarDatos();

        }
        else
        {
            mensaje.GetComponent<Text>().text = "****************** Fallo al guardar *********************";
        }

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    public void Buscar()
    {
        bool consulto = false;
        string conn = "URI=file:" + Application.dataPath + "/Plugins/Datos.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM Persona Where nombre= '"+nombre.text+"'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        
         while (reader.Read())
        {
            consulto = true;
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string apellidoC = reader.GetString(2);
            string anioC = reader.GetString(3);
            string cedulaC = reader.GetString(4);

            idGeneral = id;
            Debug.Log(name + apellidoC + anioC + cedulaC);
            mensaje.GetComponent<Text>().text = "****************** Existe *********************";
            nombre.text = name;
            apellido.text = apellidoC;
            anio.text = anioC;
            cedula.text = cedulaC;
            delete.interactable = true;
            update.interactable = true;
        }
        if (!consulto)
        {
            mensaje.GetComponent<Text>().text = "****************** No Existe *********************";
            borrarDatos();
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
