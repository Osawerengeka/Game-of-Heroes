using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using HeroesLib;

using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;



namespace Interface
{
 
    public partial class MainWindow : Window
    {
        List<Assembly> dll = new List<Assembly>();

        DirectoryInfo info;
        FileInfo[] files;
        List<Type[]> types = new List<Type[]>();

        public struct listinfo
        {
            public string fullName;
            public string showingName;
            public int pos;
            public int posincoll;
            public int posInArmy;
            public int army;
        }



        ObservableCollection<string> a = new ObservableCollection<string>();
        ObservableCollection<string> b = new ObservableCollection<string>();
        ObservableCollection<string> feat = new ObservableCollection<string>();



        bool usingMagic = false;
        string nameOfSpells;



        string select;
        string selected;
        //string ListboxElement = " ";
        ListBox l;
        Dictionary<BattleUnitStack, TextBlock> field = new Dictionary<BattleUnitStack, TextBlock>();
        Dictionary<listinfo, BattleUnitStack> list = new Dictionary<listinfo, BattleUnitStack>(); // (int)1 or 2 + pos
        Battle battle;
        BattleUnitStack[] PickingArmy = new BattleUnitStack[6];
        BattleUnitStack[] PickingArmy2 = new BattleUnitStack[6];
        private void pick(ComboBox arm)
        {

            Dictionary<string, TextBlock> textblocksarmy1 = new Dictionary<string, TextBlock>()
            {
                 { "Armyspot1",Armyspot1 },
                 { "Armyspot2",Armyspot2 },
                 { "Armyspot3",Armyspot3 },
                 { "Armyspot4",Armyspot4 },
                 { "Armyspot5",Armyspot5 },
                 { "Armyspot6",Armyspot6 },
                 { "qtspot1",qtspot1 },
                 { "qtspot2",qtspot2 },
                 { "qtspot3",qtspot3 },
                 { "qtspot4",qtspot4 },
                 { "qtspot5",qtspot5 },
                 { "qtspot6",qtspot6 },
            };
            Dictionary<string, TextBlock> textblocksarmy2 = new Dictionary<string, TextBlock>()
            {
                 { "Armyspot1",Armyspot21 },
                 { "Armyspot2",Armyspot22 },
                 { "Armyspot3",Armyspot23 },
                 { "Armyspot4",Armyspot24 },
                 { "Armyspot5",Armyspot25 },
                 { "Armyspot6",Armyspot26 },
                 { "qtspot1",qtspot21 },
                 { "qtspot2",qtspot22 },
                 { "qtspot3",qtspot23 },
                 { "qtspot4",qtspot24 },
                 { "qtspot5",qtspot25 },
                 { "qtspot6",qtspot26 },
            };
            if ((qt.Text != "") && (spot.Text != ""))
            {

                int qt_;
                int spot_;
                if ((int.TryParse(qt.Text, out qt_) && (int.TryParse(spot.Text, out spot_))))
                {
                    if ((spot_ <= 6) && (spot_ > 0) && (qt_ > 0))
                    {
                        string strSpot = "Armyspot" + spot.Text;
                        string strqt = "qtspot" + spot.Text;
                        TextBlock textS;
                        TextBlock textQ;
                        ComboBoxItem selectedplayer = (ComboBoxItem)Armyselect.SelectedItem;
                        TextBlock player = (TextBlock)selectedplayer.Content;

                        string hero = (string)ComboBoxPickingUnit.SelectedItem; ;
                        if (player.Text == "Player1")
                        {
                            textblocksarmy1.TryGetValue(strSpot, out textS);
                            textblocksarmy1.TryGetValue(strqt, out textQ);
                        }
                        else
                        {
                            textblocksarmy2.TryGetValue(strSpot, out textS);
                            textblocksarmy2.TryGetValue(strqt, out textQ);

                        }
                        textS.Text = hero.Substring(0, 1);
                        textQ.Text = qt.Text;

                        StatusofPicking.Text = StatusofPicking.Text + "\n" + "Units has been created";

                        foreach (var a in types)
                        {
                            for (int i = 0; i < a.Length; i++)
                            {

                                if (hero == a[i].Name)
                                {
                                    object obj = Activator.CreateInstance(a[i]);
                                    if (player.Text == "Player1")
                                        PickingArmy[spot_ - 1] = new BattleUnitStack(new UnitStack((Unit)obj, qt_));
                                    else
                                        PickingArmy2[spot_ - 1] = new BattleUnitStack(new UnitStack((Unit)obj, qt_));
                                    break;
                                }
                            }
                        }
                    }
                    else
                        StatusofPicking.Text = StatusofPicking.Text + "\n" + "You have only 6 slots!!!";
                }
                else
                    StatusofPicking.Text = StatusofPicking.Text + "\n" + "Something got wrong:\\ \n Try again";

            }


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            pick(Armyselect);

        }
        private void finishPicking(object sender, RoutedEventArgs e)
        {
            battle = new Battle(new BattleArmy(PickingArmy), new BattleArmy(PickingArmy2));
            StatusofPicking.Text = StatusofPicking.Text + "\n" + "You finished picking first Army";
            window1.Visibility = Visibility.Hidden;
            windowbattle.Visibility = Visibility.Visible;
            listAdding();
        }
        private void End(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);

        }

        public void colorise(BattleUnitStack u, TextBlock text)
        {
            if (u.canBeUse == true)
            {
                if (battle.player[0].army.Contains(u))
                {
                    text.Foreground = Brushes.Green;

                }
                if (battle.player[1].army.Contains(u))
                {
                    text.Foreground = Brushes.Blue;

                }
            }
            else
            {
                if (battle.player[0].army.Contains(u))
                {
                    text.Foreground = Brushes.LightGreen;

                }
                if (battle.player[1].army.Contains(u))
                {
                    text.Foreground = Brushes.LightBlue;

                }

            }
        }

        private void listAdding()
        {
            for (int i = 0; i < battle.player[0].army.Count; i++)
            {

                string str = battle.player[0].army[i].bus.Type + "0" + i.ToString();
                a.Add(battle.player[0].army[i].bus.Type);
                listinfo sct = new listinfo { fullName = str, showingName = battle.player[0].army[i].bus.Type, posincoll = a.Count - 1, pos = list.Count, posInArmy = i, army = 0 };
                list.Add(sct, battle.player[0].army[i]);
            }

            ListboxPlayer1.ItemsSource = a;


            for (int i = 0; i < battle.player[1].army.Count; i++)
            {
                string str = battle.player[1].army[i].bus.Type + "1" + i.ToString();
                b.Add(battle.player[1].army[i].bus.Type);
                listinfo sct = new listinfo { fullName = str, showingName = battle.player[1].army[i].bus.Type, posincoll = b.Count - 1, pos = list.Count, posInArmy = i, army = 1 };
                list.Add(sct, battle.player[1].army[i]);
            }
            ListboxPlayer2.ItemsSource = b;


        }

        private void endturn(BattleUnitStack unit)
        {
            if (battle.endOfTheRound() == true)
            {
                Gamelogic();
            }
            if (battle.winner == "")
            {
                battle.queue();
                listOfSpells();
                colorise(unit, field[unit]);

                unit = battle.whowillgo();

                if (unit != null)
                {
                    field[unit].Foreground = Brushes.Red;
                }
            }

        }
        private void dllparse()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var path = System.IO.Path.GetDirectoryName(location);
           // path.Remove(path.IndexOf(@"interface.exe"),14);
            info = new DirectoryInfo(path);
            files = info.GetFiles();

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(Directory.GetCurrentDirectory()+"armyspot.bmp", UriKind.Relative);
            bi3.EndInit();
            image.Stretch = Stretch.Fill;
            image.Source = bi3;

            foreach (var a in files)
            {
                if (a.Extension == ".dll")
                {
                    dll.Add(Assembly.LoadFrom(a.FullName));
                    types.Add(dll[dll.Count - 1].GetTypes());

                }
            }


            foreach (var a in types)
            {
                for (int i = 0; i < a.Length; i++)
                {

                    Regex regex = new Regex("Units");
                    string[] str = a[i].FullName.Split('.');
                    if (regex.IsMatch(str[1]))
                    {
                        ComboBoxPickingUnit.Items.Add(a[i].Name);
                    }
                }
            }

        }
        public MainWindow()
        {

            InitializeComponent();
            windowbattle.Visibility = Visibility.Hidden;
            windowbattle.IsEnabled = true;
            dllparse();
        }

        private void chooseposition(object sender, SelectionChangedEventArgs e)
        {
            var buff = ((sender as ListBox).SelectedItem as string);

            if (buff != null)
            {
                selected = buff;
                l = (ListBox)sender;
                //  ((l.SelectedItem) as TextBox).Foreground = Brushes.Gray;      
            }

        }
        int kol = 0;
        private void selectposition(object sender, MouseButtonEventArgs e)
        {
            listinfo flag = new listinfo();
            string buff = selected;

            if ((selected != " ") && (selected != null))
            {

                if (l.Name == "ListboxPlayer1")
                {
                    foreach (var i in list.Keys)
                    {
                        if ((i.showingName == selected) && (i.army == 0) && (l.SelectedIndex == i.posincoll))
                        {

                            flag = i;

                            //  a.RemoveAt(i.posincoll);                         
                        }
                    }

                }
                if (l.Name == "ListboxPlayer2")
                {
                    foreach (var i in list.Keys)
                    {
                        if ((i.showingName == selected) && (i.army == 1) && (l.SelectedIndex == i.posincoll))
                        {
                            flag = i;
                            //  b.RemoveAt(i.posincoll);
                        }
                    }

                }
                kol++;
                BattleUnitStack bat = list[flag];
                TextBlock tb = (TextBlock)sender;
                textTip(tb, bat);
                field.Add(bat, tb);
                tb.Text = buff.Substring(0, 1);
                list.Remove(flag);
                if ((a.Count() + b.Count()) == kol)
                {
                    selected = " ";
                    a.Clear();
                    b.Clear();
                    ListboxPlayer1.Visibility = Visibility.Collapsed;
                    ListboxPlayer2.Visibility = Visibility.Collapsed;
                    ListboxPlayer1.IsEnabled = false;
                    ListboxPlayer2.IsEnabled = false;
                    Gamelogic();
                }
            }
            else if (usingMagic == true)
            {
                if (spellsList[nameOfSpells].typeofmagic == "invoke")
                {
                    var unit = battle.whowillgo();
                    var finish = (TextBlock)sender;
                    if (!field.ContainsValue(finish))
                    {
                        spellsList[nameOfSpells].Doing(unit, null, battle);
                        foreach (var a in battle.player[unit.army].army)
                        {
                            if (!field.ContainsKey(a))
                            {
                                field.Add(a, finish);
                                finish.Text = a.bus.Type.Substring(0, 2);
                                textTip(finish, a);
                            }

                        }
                    }
                    endturn(unit);
                }
                else
                {
                    var unit = battle.whowillgo();
                    var finish = (TextBlock)sender;
                    if (field.ContainsValue(finish))
                    {
                        List<BattleUnitStack> a = field.Keys.ToList();
                        for (int i = 0; i < field.Keys.Count; i++)
                        {
                            if (field[a[i]] == finish)
                            {
                                spellsList[nameOfSpells].Doing(unit, a[i], battle);
                                textTip(field[a[i]], a[i]);
                                textTip(field[unit], unit);
                                if (!battle.player[a[i].army].army.Contains(a[i]))
                                {
                                    battleStatus.Text = "Was Killed";
                                    field[a[i]].Text = "";
                                    field[a[i]].ToolTip = null;

                                }
                                if (!battle.player[unit.army].army.Contains(unit))
                                {
                                    battleStatus.Text = "Was Failed";
                                    field[unit].Text = "";
                                    field[unit].ToolTip = null;
                                }

                            }

                        }
                        endturn(unit);
                    }
                }

            }
        }
        private void Gamelogic()
        {
            battle.updateUnits();
            string win = battle.winCondition(); // winning contition
            if (win != " ")
            {
                battleStatus.FontSize = 30;
                battleStatus.Text = win + "\n";

                //     Environment.Exit(0);
            }
            else
            {
                battle.round++;
                round.Text = battle.round.ToString() + " Round";
                battle.queue();
                listOfSpells();
                foreach (var a in field.Keys)
                {
                    colorise(a, field[a]);

                }
                TextBlock myKey = field[battle.whowillgo()];
                myKey.Foreground = Brushes.Red;
            }
        }

        private void textTip(TextBlock t, BattleUnitStack b)
        {
            ToolTip toolTip = new ToolTip();
            StackPanel toolTipPanel = new StackPanel();
            toolTipPanel.Children.Add(new TextBlock { Text = b.bus.Type, FontSize = 16 });
            toolTipPanel.Children.Add(new TextBlock { Text = "Quanity :" + b.bus.qty.ToString() });
            toolTipPanel.Children.Add(new TextBlock { Text = "Beat :" + ((b.bus.qty * b.bus.StandardHitpoints) + b.bus.Hitpoints).ToString() });
            List<Imod> s = b.mod;
            if (s != null)
            {
                foreach (var a in s)
                    toolTipPanel.Children.Add(new TextBlock { Text = a.Name });
            }
            else
            {
                toolTipPanel.Children.Add(new TextBlock { Text = " NO Modificators" });
            }
            toolTip.Content = toolTipPanel;
            t.ToolTip = toolTip;

        }
        private void textTip(TextBlock start, TextBlock finish, BattleUnitStack b)
        {
            finish.ToolTip = start.ToolTip;
            start.ToolTip = null;

        }
        private void textTip(TextBlock del)
        {
            del.ToolTip = null;

        }
        private void move(object sender, MouseButtonEventArgs e)
        {

            var unit = battle.whowillgo();
            TextBlock start = field[unit];

            TextBlock finish = (TextBlock)sender;
            if (field.ContainsValue(finish))
            {
                List<BattleUnitStack> a = field.Keys.ToList();
                for (int i = 0; i < field.Keys.Count; i++)
                {
                    if (field[a[i]] == finish)
                    {

                        var beatbef = a[i].bus.qty * a[i].bus.StandardHitpoints + a[i].bus.Hitpoints;
                        string s = battle.attack(unit, a[i]);
                        textTip(field[unit], unit);
                        textTip(field[a[i]], a[i]);
                        if (s == "Same Army")
                        {

                            battleStatus.Text = "Same Army. Choose another action" + "\n";
                            return;
                        }
                        else if (s == "Damaged")
                        {
                            battleStatus.Text = finish.Text + " " + s + " by " + start.Text + "\n";
                        }
                        if (s == "Killed")
                        {
                            field.Remove(a[i]);
                            textTip(finish);
                            finish.Text = "";
                            finish.Foreground = Brushes.Black;
                        }
                        if (s == "Was Failed by")
                        {
                            field.Remove(unit);
                            textTip(start);
                            start.Text = "";
                            start.Foreground = Brushes.Black;

                        }
                    }
                }
            }
            else
            {
                finish.Text = start.Text;
                field.Remove(unit);
                field.Add(unit, finish);
                start.Text = "";
                start.Foreground = Brushes.Black;
                textTip(start, finish, unit);
            }

            unit.canBeUse = false;
            endturn(unit);
        }
        private void RedGuy()
        {
            var unit = battle.whowillgo();
            if (unit != null)
            {
                field[unit].Foreground = Brushes.Red;
            }
        }
        private void waitButton(object sender, RoutedEventArgs e)
        {

            var unit = battle.whowillgo();
            battle.wait(unit);
            listOfSpells();
            colorise(unit, field[unit]);

            endturn(unit);
        }



        private void defendButton(object sender, RoutedEventArgs e)
        {
            var unit = battle.whowillgo();
            battle.defend(unit);
            listOfSpells();
            endturn(unit);
        }

        private void defeatButton(object sender, RoutedEventArgs e)
        {
            var unit = battle.whowillgo();
            battle.defeat(unit);
        }

        private void chooseabil(object sender, SelectionChangedEventArgs e)
        {
            var unit = battle.whowillgo();
            string s = ((sender as ListBox).SelectedItem as string);

            if ((s != "Default Spells:") && (s != null))
            {
                if (spellsList[s].solo == true)
                    spellsList[s].Doing(unit);
                else
                {
                    usingMagic = true;
                    nameOfSpells = s;
                }
            }
        }
        Dictionary<string, Ispell> spellsList = new Dictionary<string, Ispell>();
        private void listOfSpells()
        {
            spellsList.Clear();
            feat.Clear();
            var unit = battle.whowillgo();
            feat.Add("\nDefault Spells (Clickable) :");
            foreach (var a in unit.abl)
            {
                feat.Add(a.Name);
                spellsList.Add(a.Name, a);
            }
            abilities.ItemsSource = feat;
        }
    }
    }

