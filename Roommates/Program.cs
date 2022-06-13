using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;
using System.Linq;



namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"The chore, {c.Name} needs to get done.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for a chore"):
                        Console.Write("Chore Id: ");
                        int cId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(cId);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore: ");
                        string cName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = cName
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for a roommate"):
                        Console.Write("Roommate Id: ");
                        int rId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(rId);

                        Console.WriteLine($"{roommate.FirstName}, Rent:{roommate.RentPortion}, Room:{roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in unassignedChores)
                        {
                            Console.WriteLine($"Unassigned chore: {c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign chore to roommate"):/*work In Progreess*/
                        /*Show a list of all chores*/
                        List<Chore> allChores = choreRepo.GetAll();
                        foreach (Chore c in allChores)
                        {
                            Console.WriteLine($"{c.Id}: {c.Name}");
                        }
                        /*Select Id of chore you want to assign*/
                        Console.WriteLine("");
                        Console.Write("Type the number of the chore you would like to assign and press 'Enter'");
                        int chosenChoreId = Int32.Parse(Console.ReadLine());

                        /*Show a list of all roommates*/
                        Console.WriteLine("");
                        List<Roommate> roommates = roommateRepo.GetAll();
                        foreach (Roommate r in roommates)
                        {
                            Console.WriteLine($"{r.Id}: {r.FirstName} {r.LastName} | Rent:{r.RentPortion} | Room:{r.Room.Name}");
                        }

                        /*Prompt user to choose a roommate to assign the chore to*/
                        Console.Write("Type the number of the Roommate you would like to assign the chore to.");
                        int chosenRoommateId = Int32.Parse(Console.ReadLine());

                        /*Assign the chore*/
                        choreRepo.AssignChoreToRoommate(chosenChoreId, chosenRoommateId);

                        /*Let the user know the operation was successfull*/
                        Console.WriteLine("Chore successfully added!");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Get chore counts"):
                        List<Load> choreLoadByRoommate = choreRepo.GetChoreCounts();
                        foreach(Load l in choreLoadByRoommate )
                        {
                            Console.WriteLine($"{l.RoommateName}: {l.ChoreLoad}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a room"):
                        List<Room> roomList = roomRepo.GetAll();
                        foreach (Room r in roomList)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }
                        Console.Write("Which room would you like to delete? ");
                        int deleteRoomId = int.Parse(Console.ReadLine());
                        roomRepo.Delete(deleteRoomId);
                        Console.WriteLine("");
                        Console.WriteLine("Room successfully deleted");

                        Console.WriteLine("");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach (Chore c in choreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreOptions.FirstOrDefault(c => c.Id == selectedChoreId);

                        Console.Write("New Chore Description: ");
                        selectedChore.Name = Console.ReadLine();

                        choreRepo.Update(selectedChore);

                        Console.WriteLine("The chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a chore"):
                        List<Chore> dChoreList = choreRepo.GetAll();
                        foreach (Chore c in dChoreList)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write("Which chore would you like to delete? ");
                        int deleteChoreId = int.Parse(Console.ReadLine());
                        choreRepo.Delete(deleteChoreId);
                        Console.WriteLine("");
                        Console.WriteLine("Chore successfully deleted");

                        Console.WriteLine("");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Reassign a chore"):
                        List<AssignedChore> assignedChores = choreRepo.GetAssignedChores();
                        for (int x = 0; x < assignedChores.Count; x++)
                        {
                            Console.WriteLine($"{x+1}. {assignedChores[x].ChoreName} -- {assignedChores[x].RoommateName}");
                        }
                        Console.WriteLine("");

                        //Use this to access the index of the Assigned chore. Remember to -1
                        Console.WriteLine("Choose a chore to reassign");
                        int userAssignedChoreChoice = Int32.Parse(Console.ReadLine())-1;
                        Console.WriteLine("");
                        Console.WriteLine("Which Roommate would you like to assign this chore to?");
                        List<Roommate> roommatesToAssignChore = roommateRepo.GetAll();
                        foreach(Roommate r in roommatesToAssignChore)
                        {
                            Console.WriteLine($"{r.Id}). {r.FirstName} {r.LastName}");
                        }
                        Console.WriteLine("Type the number of the roommate you would like to assign the chore to.");
                        int userRoommateChoice = Int32.Parse(Console.ReadLine());

                        //Now we use the chore Id and Roommate Id to add a new entry to RoommateChore, after deleting the old entry.
                        //To Delete, use the value from userAssignedChoreChoice
                        int choreAssignemntToDelete = assignedChores[userAssignedChoreChoice].Id;
                        choreRepo.DeleteChoreAssignment(choreAssignemntToDelete);
                        
                        //We'll add to the RoommateChore Table, new RoommateId and Chore ID entry
                        //RoommateId: this is userRoommateChoice
                        int ChoreToReassign = assignedChores[userAssignedChoreChoice].ChoreId;
                        choreRepo.AssignChoreToRoommate(ChoreToReassign, userRoommateChoice);
                        Console.WriteLine("Chore succesfully re-assigned");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Show all chores",
                "Search for a chore",
                "Add a chore",
                "Search for a roommate",
                "Show all unassigned chores",
                "Assign chore to roommate",
                "Get chore counts",
                "Update a room",
                "Delete a room",
                "Update a chore",
                "Delete a chore",
                "Reassign a chore",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }

    }
}
