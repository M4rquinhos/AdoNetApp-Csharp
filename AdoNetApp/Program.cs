//Cadena de conexion ó Connection String
using AdoNetApp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;

using IHost host = Host.CreateDefaultBuilder(args).Build();

var configuracion = host.Services.GetService<IConfiguration>();

var cadenaConexion = configuracion?.GetConnectionString("CadenaConexion");

//var cadenaConexion = "Data Source=DESKTOP-4DN5MOQ;Database=AdoNetDemo;Integrated Security=True;TrustServerCertificate=True";

//Por si se necesita user y pass
//var cadenaConexion2 = "Data Source=DESKTOP-4DN5MOQ;Database=AdoNetDemo;Integrated Security=False;User Id=miUsuario;Password=miPassword";

//Console.WriteLine("Escribe le nombre de la persona a ingresar a la base de datos: ");
//var nombre = Console.ReadLine();

try
{
	using (SqlConnection conexion = new SqlConnection(cadenaConexion))
	{
		//Abrir la conexion
		conexion.Open();
		Console.WriteLine("Conexion Abierta");

        //Ejemplo de una query hardcodeada
        //var query = @"INSERT INTO Personas (Nombre)
        //									VALUES (@nombre)";

        //var query = "Personas_Insertar";
        //var query = "SELECT COUNT(*) FROM Personas";
        //var query = "Personas_Leer";
        var query = "Personas_Y_Productos";

        using (SqlCommand comando = new SqlCommand(query, conexion))
		{
            //conviertiendo la query o el comando en un Procedimiento almacenado
            //comando.CommandType = CommandType.StoredProcedure;
            //parametro para la query
            //comando.Parameters.Add(new SqlParameter("@nombre", nombre));

            //parametro para recibir el Id
            //var parametroId = new SqlParameter
            //{
            //	ParameterName = "@id",
            //	Direction = ParameterDirection.Output,
            //	DbType = DbType.Int32
            //};
            //comando.Parameters.Add(parametroId);

            //Ejecutando la query de manera asincrona
            //await comando.ExecuteNonQueryAsync();
            //var cantidadRegistros = await comando.ExecuteScalarAsync();
            //Console.WriteLine($"La cantidad de registros en la tabla son: {cantidadRegistros}");
            //Console.WriteLine($"Filas afectadas: {filasAfectadas}");

            //var id = (int)parametroId.Value;
            //Console.WriteLine($"id de la persona: {id}");


            //DataTable

            //DataTable y procedimientos almacenados
            comando.CommandType = CommandType.StoredProcedure;

            using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
            {
                //var dt = new DataTable();
                //adaptador.Fill(dt);

                var ds = new DataSet();
                adaptador.Fill(ds);

                //foreach (DataRow fila in dt.Rows)
                //{
                //    //Console.WriteLine($"{fila[0]} | {fila[1]}");
                //    Console.WriteLine($"{fila["Id"]} | {fila["Nombre"]}");
                //}

                //DataTable a listado
                //var personas = dt.AsEnumerable().Select(fila => new Persona
                //{ 
                //    Id = fila.Field<int>("Id"),
                //    Nombre = fila.Field<string>("Nombre")!
                //}).ToList();

                var tablaPersonas = ds.Tables[0];
                var tablasProductos = ds.Tables[1];

                Console.WriteLine("---Tabla Personas---");
                foreach (DataRow fila in tablaPersonas.Rows)
                {
                    Console.WriteLine($"{fila["Id"]} | {fila["Nombre"]}");
                }


                Console.WriteLine("---Tabla Productos---");
                foreach (DataRow fila in tablasProductos.Rows)
                {
                    Console.WriteLine($"{fila["Id"]} | {fila["Nombre"]} | {fila["Precio"]}");
                }


            }

            

        }
	}
}
catch (Exception ex)
{
	Console.WriteLine("no se armó, pa");
	Console.WriteLine(ex.Message);
}

Console.WriteLine("Fin");


await host.RunAsync();