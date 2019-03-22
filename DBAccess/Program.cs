using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace DBAccess
{
    class Program
    {
        private OleDbConnection connection = new OleDbConnection();//creates connection object
        static void Main(string[] args)
        {
            Program temp = new Program();//creates an instance of program called temp
            temp.DBSelectAll();//calls DBSelectAll method of the instance of program
            String x = "0";//string variable used by user to navigate console inputs
            String name;//stores user input to be passed to query
            String nameTarget;
            String DOB;
            String address;
            while (x != "4")
            {
                Console.WriteLine("Press 1 to insert a row into the table, 2 to update a name in the table, 3 to delete a row or 4 to exit the program");
                x = Console.ReadLine();
                if (x == "1") {
                    Console.WriteLine("Please enter the name of the client you wish to add.");
                    name = Console.ReadLine();
                    Console.WriteLine("Please enter the Date of birth in MM/DD/YYYY format of the client you wish to add.");
                    DOB = Console.ReadLine();
                    Console.WriteLine("Please enter the Address of the client you wish to add.");
                    address = Console.ReadLine();
                    temp.DBInsert(name,DOB,address);//calls DBInsert method of the instance of program
                    temp.DBSelectAll();//shows updated table
                }//insert row if statement
                else if (x == "2") {
                    Console.WriteLine("Please enter the name of the client you wish to change.");
                    nameTarget = Console.ReadLine();
                    Console.WriteLine("Please enter the new name for this client.");
                    name = Console.ReadLine();
                    temp.DBUpdate(nameTarget, name);//passes user inputs to DBUpdate method
                    temp.DBSelectAll();//shows updated table
                }//update name if statement
                else if (x == "3") {
                    Console.WriteLine("Please enter the id of the client you wish to delete.");
                    name = Console.ReadLine();
                    temp.DBDelete(name);
                    temp.DBSelectAll();//shows updated table
                }//delete row if statement
                else if (x == "4") { }//if 4 do nothing and then reach end of program
                else { Console.WriteLine("Press 1 to insert a field, 2 to update an field, 3 to delete an field or 4 to exit the program");}//reprompt user
            }//while user x input != 4 run through user console interface
        }//end of main method

        private void ConnectionOpen()
        {
                connection.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Users\PC\Documents\Database1.accdb; Persist Security Info = False";
                //***^^^update data source if database file changes directory^^^***
                try
                {
                    connection.Open();//close the connection within methods that call connectionOpen
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }//method to open connection to access database file. Does not close the connection automatically. Update if database directory changes
        //***^^^Update if database directory changes^^^***
        private void DBSelectAll()
        {
            using (OleDbCommand command = new OleDbCommand())//create command object
            {
                ConnectionOpen();//open connection
                command.Connection = connection;//sets command connection value
                command.CommandText = "Select * From Client_Info";//sets command command text
                OleDbDataReader reader = command.ExecuteReader();//creates reader object
                while (reader.Read())//while there are more entries
                {
                    Console.WriteLine(reader["Client_ID"].ToString() + " " + reader["Client_Name"].ToString() + " " + reader["DOB"].ToString() + " " + reader["Address"].ToString());
                }
                connection.Close();//close connection
            }
        }//method to query database for all rows in Client_Info table

        private void DBInsert(String name, String DOB, String address)
        {
            using (OleDbCommand command = new OleDbCommand())//create command object
            {
                ConnectionOpen();//call connection method
                command.Connection = connection;//sets command connection value
                command.CommandText = "Insert Into Client_Info (Client_Name, DOB, Address) Values (?,?,?)";
                command.Parameters.AddWithValue("Client_Name", name);
                command.Parameters.AddWithValue("DOB", DOB);
                command.Parameters.AddWithValue("Address", address);
                command.ExecuteNonQuery();//execute insert query
                connection.Close();//close connection
            }
        }//method to insert row into Client_Info table

        private void DBUpdate(String Target, String Name)
        {
            using (OleDbCommand command = new OleDbCommand())//create command object
            {
                ConnectionOpen();
                command.Connection = connection;//sets command connection value
                command.CommandText = "Update Client_Info Set Client_Name = ? Where Client_Name = ?";
                command.Parameters.AddWithValue("Client_Name", Name);
                command.Parameters.AddWithValue("Client_Name", Target);
                command.ExecuteNonQuery();//execute update query
                connection.Close();
            }
        }//method to update the name of a client

        private void DBDelete(String ID)
        {
            using (OleDbCommand command = new OleDbCommand())//create command object
            {
                ConnectionOpen();
                command.Connection = connection;//sets command connection value
                command.CommandText = "Delete * From Client_Info Where Client_ID = ?";
                command.Parameters.AddWithValue("Client_ID", ID);
                command.ExecuteNonQuery();//execute delete query
                connection.Close();
            }
        }//method to delete a row from the Client_Info table
    }
}
