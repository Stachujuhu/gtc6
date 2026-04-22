using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO;
using Avalonia.Input;
using System.Threading;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using System.Text.Json;

namespace GTC6;

public class Person
{
	public string Name { get; set; }
}

public partial class MainWindow : Window
{
    static string json = File.ReadAllText("Assets/character.json");
	Person person = JsonSerializer.Deserialize<Person>(json);

	//playernam.Source = new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(playername)); 
	int moneys = 0;  // reset money
    string carpath = "car1";  // set the carpath so dotnet doesn't crash out
    bool started = false; // stop players from restarting the game forcefully
    public MainWindow()
    {
        InitializeComponent();
        this.AttachedToVisualTree += (_, __) => this.Focus(); // focus on the window
    }

    public void OnKeyDown(object? sender, KeyEventArgs e)
    {
        var playername = new Uri($"avares://GTC6/Assets/{person.Name}.jpg");
        playernam.Source = new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(playername));
        double x = Canvas.GetLeft(player); // get the X position of the player
        double y = Canvas.GetTop(player); // get the Y position of the player

        double carx = Canvas.GetLeft(car); // get the positions of the car
        double cary = Canvas.GetTop(car);
        Random random = new Random(); // generate random for use with all shit
        double randomx = random.Next(0, 600); // generate random positions of the car
        double randomy = random.Next(0, 600);

        switch (e.Key)  // change position on key press
        {
            case Key.W:
            y -= 5;
            break;
            case Key.S:
            y += 5;
            break;
            case Key.A:
            x -= 5;
            break;
            case Key.D:
            x += 5;
            break;
            case Key.F:
            while (started == false){ // start the game
            Canvas.SetLeft(car, randomx);
            Canvas.SetTop(car, randomy);
            car.IsVisible=true;
            started = true; // stop players from starting the game again
            }
            break;
        }
        Canvas.SetLeft(player, x);  // set players position based on key press
        Canvas.SetTop(player, y);

        if (Canvas.GetLeft(player) > (800 + player.Width))  // teleport onto the other side when on edge
        {
            Canvas.SetLeft(player, 0);
        }
        if (Canvas.GetLeft(player) < (0 - player.Width))
        {
            Canvas.SetLeft(player, 800);
        }

        if ( y > (800 + player.Height))
        {
            Canvas.SetTop(player, 0);
        }
        if ( y < (0 - player.Height))
        {
            Canvas.SetTop(player, 800);
        }
        if ((carx + car.Width) >= x && (cary + car.Height) >= y && (carx - 80) <= x && (cary - 80) <= y && car.IsVisible == true)
        {
            int carnumber = random.Next(1,3);     // Stealing the car
            Canvas.SetLeft(car, randomx);   //Respawning the car at random position
            Canvas.SetTop(car, randomy);
            if (carpath == "car1")      //give player money based on the car
            {
                moneys += 1500;
            }
            if (carpath == "car2")
            {
                moneys += 2000;
            }
            carpath = $"car{carnumber}";   
            var uri = new Uri($"avares://GTC6/Assets/{carpath}.jpg");

            picture.Source = new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(uri));      //Change the picture to random (can be the same)
            //Console.WriteLine(carpath); //DEBUG!!
        }
        //cords.Text=($"(DEBUG) Moved to ({x}, {y}). Pandziucha is at ({carx}, {cary})");   // show coordinates where the player and the car is (DEBUG)
        money.Text=($"You have {moneys} moneys."); //Display the amount of money the player has
    }
}
