// MainWindow.xaml.cs
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Damage_Calc
{

    public partial class MainWindow : Window
    {
        private readonly List<List<Armor>> armorList = new List<List<Armor>>() {
            new List<Armor>
            {
                //type, armor points, armor toughness
                new Armor("None",       0, 0),
                new Armor("Leather",    1, 0),
                new Armor("Gold",       2, 0),
                new Armor("Chainmail",  2, 0),
                new Armor("Iron",       2, 0),
                new Armor("Diamond",    3, 2),
                new Armor("Netherite",  3, 3),
                new Armor("Turtle",     2, 0)
            },
            new List<Armor>
            {
                new Armor("None",       0, 0),
                new Armor("Leather",    3, 0),
                new Armor("Gold",       5, 0),
                new Armor("Chainmail",  5, 0),
                new Armor("Iron",       6, 0),
                new Armor("Diamond",    8, 2),
                new Armor("Netherite",  8, 3)
            },
            new List<Armor>
            {
                new Armor("None",       0, 0),
                new Armor("Leather",    2, 0),
                new Armor("Gold",       3, 0),
                new Armor("Chainmail",  4, 0),
                new Armor("Iron",       5, 0),
                new Armor("Diamond",    6, 2),
                new Armor("Netherite",  6, 3)
            },
            new List<Armor>
            {
                new Armor("None",       0, 0),
                new Armor("Leather",    1, 0),
                new Armor("Gold",       1, 0),
                new Armor("Chainmail",  1, 0),
                new Armor("Iron",       2, 0),
                new Armor("Diamond",    3, 2),
                new Armor("Netherite",  3, 3)
            }
        };

        public MainWindow()
        {
            InitializeComponent();
        }
        private void WeaponComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedWeapon = ((ComboBoxItem)WeaponComboBox.SelectedItem)?.Content.ToString();

            if (selectedWeapon == "Mace")
            {
                SharpnessPanel.Visibility = Visibility.Collapsed;
                MaceOptionsPanel.Visibility = Visibility.Visible;
                UpdateMaceLevelComboBox(); // default to Density
            }
            else
            {
                SharpnessPanel.Visibility = Visibility.Visible;
                MaceOptionsPanel.Visibility = Visibility.Collapsed;
            }
        }
        private void MaceModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMaceLevelComboBox();
        }

        private void UpdateMaceLevelComboBox()
        {
            string mode = ((ComboBoxItem)MaceModeComboBox.SelectedItem)?.Content.ToString();
            MaceLevelComboBox.Items.Clear();

            int maxLevel = mode == "Breach" ? 4 : 5;

            for (int i = 0; i <= maxLevel; i++)
            {
                MaceLevelComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
            }

            MaceLevelComboBox.SelectedIndex = 0;
        }
        private void CalculateDamage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string weapon = ((ComboBoxItem)WeaponComboBox.SelectedItem)?.Content.ToString();
                bool isCritical = IsCriticalCheckBox.IsChecked == true;
                double critMult = 1;
                double initialDamage = 0;
                if (isCritical)
                {
                    critMult = 1.5;
                }
                bool breach = false;
                if (weapon == "Mace")
                {
                    // Placeholder: implement Mace damage logic here
                    int baseDamage = 6;

                    string enchant = ((ComboBoxItem)MaceModeComboBox.SelectedItem)?.Content.ToString();
                    int enchantLvl = int.TryParse(((ComboBoxItem)MaceLevelComboBox.SelectedItem)?.Content.ToString(), out int lvl) ? lvl : 0;
                    int blocksDropped = int.TryParse(BlocksDroppedInput.Text, out int bd) ? bd : 0;
                    double bonusDamage = 0;
                    if (enchant.Equals("Density"))
                    {
                        bonusDamage = 0.5 * blocksDropped * enchantLvl;
                        breach = false;
                    } else
                    {
                        breach = true;
                    }
                    initialDamage = baseDamage + bonusDamage; 
                    for (; blocksDropped > 0; blocksDropped--)
                    {
                        if (blocksDropped <= 3)
                        {
                            initialDamage += 4;
                        }
                        else if (blocksDropped >= 4 && blocksDropped <= 9)
                        {
                            initialDamage += 2;
                        } else
                        {
                            initialDamage += 1;
                        }
                    }
                    initialDamage *= critMult;
                  
                }
                else
                {
                    // sharpness and sword/axe logic 
                    int sharpness = int.Parse(((ComboBoxItem)SharpnessComboBox.SelectedItem)?.Content.ToString() ?? "0");
                    if (sharpness == 0) { sharpness--; }

                    double baseDamage = weapon switch
                    {
                        "Wooden Sword" => 4,
                        "Stone Sword" => 5,
                        "Iron Sword" => 6,
                        "Diamond Sword" => 7,
                        "Netherite Sword" => 8,
                        "Wooden Axe" => 7,
                        "Stone Axe" => 9,
                        "Iron Axe" => 9,
                        "Diamond Axe" => 9,
                        "Netherite Axe" => 10,
                        _ => 1
                    };
                    initialDamage = critMult * baseDamage + (0.5 * (sharpness + 1));
                }

                string[] armorType = new string[4];
                armorType[0] = ((ComboBoxItem)HelmetComboBox.SelectedItem)?.Content.ToString();
                armorType[1] = ((ComboBoxItem)ChestComboBox.SelectedItem)?.Content.ToString();
                armorType[2] = ((ComboBoxItem)LegsComboBox.SelectedItem)?.Content.ToString();
                armorType[3] = ((ComboBoxItem)BootsComboBox.SelectedItem)?.Content.ToString();
                int armorPoints = 0;
                int armorToughness = 0;
                for (int i = 0; i < armorType.Length; i++)
                {
                    switch (armorType[i])
                    {
                        case "None":
                            armorPoints += armorList[i][0].ArmorPoints;
                            armorToughness += armorList[i][0].ArmorToughness;
                            break;
                        case "Leather":
                            armorPoints += armorList[i][1].ArmorPoints;
                            armorToughness += armorList[i][1].ArmorToughness;
                            break;
                        case "Gold":
                            armorPoints += armorList[i][2].ArmorPoints;
                            armorToughness += armorList[i][2].ArmorToughness;
                            break;
                        case "Chainmail":
                            armorPoints += armorList[i][3].ArmorPoints;
                            armorToughness += armorList[i][3].ArmorToughness;
                            break;
                        case "Iron":
                            armorPoints += armorList[i][4].ArmorPoints;
                            armorToughness += armorList[i][4].ArmorToughness;
                            break;
                        case "Diamond":
                            armorPoints += armorList[i][5].ArmorPoints;
                            armorToughness += armorList[i][5].ArmorToughness;
                            break;
                        case "Netherite":
                            armorPoints += armorList[i][6].ArmorPoints;
                            armorToughness += armorList[i][6].ArmorToughness;
                            break;
                        case "Turtle":
                            armorPoints += armorList[i][7].ArmorPoints;
                            armorToughness += armorList[i][7].ArmorToughness;
                            break;
                    }

                }
                int protTotalLvls = 0;
                protTotalLvls += int.TryParse(((ComboBoxItem)HelmetProtComboBox.SelectedItem)?.Content.ToString(), out int helmetLvl) ? helmetLvl : 0;
                protTotalLvls += int.TryParse(((ComboBoxItem)ChestProtComboBox.SelectedItem)?.Content.ToString(), out int chestLvl) ? chestLvl : 0;
                protTotalLvls += int.TryParse(((ComboBoxItem)LegsProtComboBox.SelectedItem)?.Content.ToString(), out int legsLvl) ? legsLvl : 0;
                protTotalLvls += int.TryParse(((ComboBoxItem)BootsProtComboBox.SelectedItem)?.Content.ToString(), out int bootsLvl) ? bootsLvl : 0;

                double armorReduction;
                int breachLevel = 0;

                armorReduction = 1 - Math.Min(20, Math.Max(armorPoints / 5, armorPoints - (4 * initialDamage) / (armorToughness + 8))) / 25;
                
                if (breach)
                {
                    breachLevel = int.TryParse(((ComboBoxItem)MaceLevelComboBox.SelectedItem)?.Content.ToString(), out int bl) ? bl : 0;
                    armorReduction = Math.Max(0, armorReduction-breachLevel*0.15);
                }
               
                
                double protReduction = 1 - (0.04 * protTotalLvls);
                double totalReduction = armorReduction * protReduction;
                double finalDamage = initialDamage * (totalReduction);

                ResultTextBlock.Text = $"Calculated Damage: {finalDamage:F2}";
            }



            catch (Exception ex)
            {
                ResultTextBlock.Text = $"Error: {ex.Message}";
            }
        }
    }
}
