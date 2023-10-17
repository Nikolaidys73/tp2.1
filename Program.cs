using System;
using System.Collections.Generic;
using System.Linq;

public enum VehicleSize { Mini, Standard, Max }

public class Vehicle
{
    public required string Model { get; set; }
    public required string OwnerDNI { get; set; }
    public required string LicensePlate { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public VehicleSize Size { get; set; }
}


public class ParkingSystem
{
    private List<Vehicle> regularParking = new List<Vehicle>();
    private List<Vehicle> quantumParking = new List<Vehicle>();
    private List<Vehicle> temporalParking = new List<Vehicle>();

    public void ListAllVehicles()
    {
        Console.WriteLine("Regular Parking:");
        foreach (var vehicle in regularParking)
        {
            Console.WriteLine($"{vehicle.Model} - {vehicle.LicensePlate} - {vehicle.OwnerDNI}");
        }

        Console.WriteLine("Quantum Parking:");
        foreach (var vehicle in quantumParking)
        {
            Console.WriteLine($"{vehicle.Model} - {vehicle.LicensePlate} - {vehicle.OwnerDNI}");
        }
    }

    public void AddVehicle()
    {
        // Generar atributos aleatorios para el nuevo vehículo.
        Random random = new Random();
        string[] models = { "Modelo A", "Modelo B", "Modelo C" };
        //string[] owners = { "DNI1", "DNI2", "DNI3" };
        string licensePlate = Guid.NewGuid().ToString().Substring(0, 7);
        double length = random.NextDouble() * 3 + 2; // Valor entre 2 y 5 metros.
        double width = random.NextDouble() * 0.5 + 1;  // Valor entre 1 y 1.5 metros.
        VehicleSize size = (VehicleSize)random.Next(3);   
        string ownerDNI = random.Next(10000000, 99999999).ToString();  // Generar un número de DNI aleatorio de hasta 8 dígitos.

        var newVehicle = new Vehicle
        {
            Model = models[random.Next(models.Length)],
            OwnerDNI = ownerDNI,
            LicensePlate = licensePlate,
            Length = length,
            Width = width,
            Size = size
        };

        if (size == VehicleSize.Mini)
        {
            if (regularParking.Count < 12)
            {
                regularParking.Add(newVehicle);
            }
            else
            {
                quantumParking.Add(newVehicle);
            }
        }
        else
        {
            quantumParking.Add(newVehicle);
        }
    }

    public void RemoveVehicleByLicensePlate(string licensePlate)
    {
        regularParking.RemoveAll(vehicle => vehicle.LicensePlate == licensePlate);
        quantumParking.RemoveAll(vehicle => vehicle.LicensePlate == licensePlate);
        temporalParking.RemoveAll(vehicle => vehicle.LicensePlate == licensePlate);
    }

    public void RemoveVehicleByOwnerDNI(string ownerDNI)
    {
        regularParking.RemoveAll(vehicle => vehicle.OwnerDNI == ownerDNI);
        quantumParking.RemoveAll(vehicle => vehicle.OwnerDNI == ownerDNI);
        temporalParking.RemoveAll(vehicle => vehicle.OwnerDNI == ownerDNI);
    }

public void RemoveRandomVehicles()
    {
        var random = new Random();
        int maxToRemove = Math.Min(regularParking.Count + quantumParking.Count, 10);

        int count = random.Next(1, maxToRemove + 1); // aca se elije la cantidad aleatorea de vehiculos a eliminar
        
        for (int i = 0; i < count; i++)
        {
            if (regularParking.Count + quantumParking.Count == 0)
                break;

            int parkingChoice = random.Next(2);
            if (parkingChoice == 0 && regularParking.Count > 0)
            {
                temporalParking.Add(regularParking.First());
                regularParking.RemoveAt(0);
            }
            else if (quantumParking.Count >  0)
            {
                temporalParking.Add(quantumParking.First());
                quantumParking.RemoveAt(0);
            }
        }
    }


    public void OptimizeSpace()
    {
        // Mover todos los vehículos de estacionamiento temporal de regreso a su espacio original.
        regularParking.AddRange(temporalParking.Where(v => v.Size == VehicleSize.Mini && regularParking.Count < 12));
        quantumParking.AddRange(temporalParking.Where(v => v.Size != VehicleSize.Mini));
        temporalParking.Clear();
    }
}

class Program
{
    static void Main(string[] args)
    {
        ParkingSystem parkingSystem = new ParkingSystem();

        while (true)
        {
            Console.WriteLine($"1) Listar todos los vehículos");
            Console.WriteLine($"2) Agregar un nuevo vehículo");
            Console.WriteLine($"3) Remover un vehículo por matrícula");
            Console.WriteLine($"4) Remover un vehículo por DNI del dueño");
            Console.WriteLine($"5) Remover una cantidad aleatoria de vehículos");
            Console.WriteLine($"6) Optimizar espacio");
            Console.WriteLine($"7) Salir");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    parkingSystem.ListAllVehicles();
                    break;
                case "2":
                    parkingSystem.AddVehicle();
                    break;
                case "3":
                    Console.Write("Introduce la matrícula del vehículo a remover: ");
                    string licensePlate = Console.ReadLine();
                    parkingSystem.RemoveVehicleByLicensePlate(licensePlate);
                    break;
                case "4":
                    Console.Write("Introduce el DNI del dueño del vehículo a remover: ");
                    string ownerDNI = Console.ReadLine();
                    parkingSystem.RemoveVehicleByOwnerDNI(ownerDNI);
                    break;
                case "5":
                    Console.Write("Remover autos Random: ");
                    int count = int.Parse(Console.ReadLine());
                    parkingSystem.RemoveRandomVehicles();
                    break;
                case "6":
                    parkingSystem.OptimizeSpace();
                    break;
                case "7":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                    break;
            }
        }
    }
}
