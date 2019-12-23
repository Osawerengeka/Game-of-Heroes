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
        readonly List<Assembly> dll = new List<Assembly>();
        DirectoryInfo info;
        FileInfo[] files;
        readonly List<Type[]> types = new List<Type[]>();
        public struct Listinfo
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
        string selected;
        //string ListboxElement = " ";
        ListBox l;
        Dictionary<BattleUnitStack, TextBlock> field = new Dictionary<BattleUnitStack, TextBlock>();
        Dictionary<Listinfo, BattleUnitStack> list = new Dictionary<Listinfo, BattleUnitStack>(); // (int)1 or 2 + pos

        Dictionary<(int, int), TextBlock> back = new Dictionary<(int, int), TextBlock>();

        Battle battle;
        BattleUnitStack[] PickingArmy = new BattleUnitStack[6];
        BattleUnitStack[] PickingArmy2 = new BattleUnitStack[6];
        private void Pick(ComboBox arm)
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
                        if (hero != null)
                        {
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

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Pick(Armyselect);

        }
        private void FinishPicking(object sender, RoutedEventArgs e)
        {
            battle = new Battle(new BattleArmy(PickingArmy), new BattleArmy(PickingArmy2));
            StatusofPicking.Text = StatusofPicking.Text + "\n" + "You finished picking first Army";
            window1.Visibility = Visibility.Hidden;
            windowbattle.Visibility = Visibility.Visible;
            ListAdding();
        }
        private void End(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);

        }

        public void Colorise(BattleUnitStack u, TextBlock text)
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

        private void ListAdding()
        {
            for (int i = 0; i < battle.player[0].army.Count; i++)
            {

                string str = battle.player[0].army[i].bus.Type + "0" + i.ToString();
                a.Add(battle.player[0].army[i].bus.Type);
                Listinfo sct = new Listinfo { fullName = str, showingName = battle.player[0].army[i].bus.Type, posincoll = a.Count - 1, pos = list.Count, posInArmy = i, army = 0 };
                list.Add(sct, battle.player[0].army[i]);
            }

            ListboxPlayer1.ItemsSource = a;


            for (int i = 0; i < battle.player[1].army.Count; i++)
            {
                string str = battle.player[1].army[i].bus.Type + "1" + i.ToString();
                b.Add(battle.player[1].army[i].bus.Type);
                Listinfo sct = new Listinfo { fullName = str, showingName = battle.player[1].army[i].bus.Type, posincoll = b.Count - 1, pos = list.Count, posInArmy = i, army = 1 };
                list.Add(sct, battle.player[1].army[i]);
            }
            ListboxPlayer2.ItemsSource = b;


        }

        private void Endturn(BattleUnitStack unit)
        {

            Cleansteps();
            if (battle.EndOfTheRound() == true)
            {
                Gamelogic();
            }
            if (battle.winner == "")
            {
                battle.Queue();
                ListOfSpells();
                Colorise(unit, field[unit]);

                unit = battle.Whowillgo();
                Putsteps(unit.bus.Step, field[unit]);
                if (unit != null)
                {
                    field[unit].Foreground = Brushes.Red;
                }

            }

        }
        private void Dllparse()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var path = System.IO.Path.GetDirectoryName(location);
            // path.Remove(path.IndexOf(@"interface.exe"),14);
            info = new DirectoryInfo(path);
            files = info.GetFiles();

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
            Dllparse();
        }

        private void Chooseposition(object sender, SelectionChangedEventArgs e)
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
        private void Selectposition(object sender, MouseButtonEventArgs e)
        {
            Listinfo flag = new Listinfo();
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
                

                if (flag.fullName != null)
                {
                    kol++;
                    BattleUnitStack bat = list[flag];

                    TextBlock tb = (TextBlock)sender;
                    TextTip(tb, bat);
                    field.Add(bat, tb);
                    tb.Text = buff.Substring(0, 1);
                    list.Remove(flag);                   
                }
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
                    var unit = battle.Whowillgo();
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
                                TextTip(finish, a);
                            }

                        }
                    }
                    Endturn(unit);
                }
                else
                {
                    var unit = battle.Whowillgo();
                    var finish = (TextBlock)sender;
                    if (field.ContainsValue(finish))
                    {
                        List<BattleUnitStack> a = field.Keys.ToList();
                        for (int i = 0; i < field.Keys.Count; i++)
                        {
                            if (field[a[i]] == finish)
                            {
                                spellsList[nameOfSpells].Doing(unit, a[i], battle);
                                TextTip(field[a[i]], a[i]);
                                TextTip(field[unit], unit);
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
                        Endturn(unit);
                    }
                }

            }
        }
        private void Gamelogic()
        {
            battle.UpdateUnits();
            string win = battle.WinCondition(); // winning contition
            if (win != " ")
            {
                battleStatus.FontSize = 30;
                battleStatus.Text = win + "\n";
                defeat.IsEnabled = false;
                defend.IsEnabled = false;
                wait.IsEnabled = false;
                //     Environment.Exit(0);
            }
            else
            {
                battle.round++;
                round.Text = battle.round.ToString() + " Round";
                battle.Queue();
                ListOfSpells();
                foreach (var a in field.Keys)
                {
                    Colorise(a, field[a]);

                }
                TextBlock myKey = field[battle.Whowillgo()];
                Putsteps(battle.Whowillgo().bus.Step, field[battle.Whowillgo()]);
                myKey.Foreground = Brushes.Red;
            }
        }

        private void TextTip(TextBlock t, BattleUnitStack b)
        {
            ToolTip toolTip = new ToolTip();
            StackPanel toolTipPanel = new StackPanel();
            toolTipPanel.Children.Add(new TextBlock { Text = b.bus.Type, FontSize = 16 });
            toolTipPanel.Children.Add(new TextBlock { Text = "Quanity :" + b.bus.qty.ToString() });
            toolTipPanel.Children.Add(new TextBlock { Text = "Beat :" + ((b.bus.qty * b.bus.StandardHitpoints) + b.bus.Hitpoints).ToString() });
            toolTipPanel.Children.Add(new TextBlock { Text = "Range :" + b.bus.Range.ToString() });

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
        private void TextTip(TextBlock start, TextBlock finish, BattleUnitStack b)
        {
            finish.ToolTip = start.ToolTip;
            start.ToolTip = null;

        }
        private void TextTip(TextBlock del)
        {
            del.ToolTip = null;

        }

        private int Distance(TextBlock a, TextBlock b)
        {
            var fir = a.Name.Substring(1);
            var sec = b.Name.Substring(1);
            int val11 = int.Parse(fir.Substring(0, 1));
            int val12 = int.Parse(sec.Substring(0, 1));
            int val21 = int.Parse(fir.Substring(1));
            int val22 = int.Parse(sec.Substring(1));

            int res = Math.Max(Math.Abs(val12 - val11), Math.Abs(val21 - val22));


            // int res =(int) Math.Sqrt((val12 - val11) ^ 2 + (val22 - val21) ^ 2);
            return res;
        }

        private void Putsteps(int step, TextBlock u)
        {
            string s = u.Name.Substring(1);
            var sy = s.Substring(0, 1);
            var sx = s.Substring(1);

            int.TryParse(sy, out int y);
            int.TryParse(sx, out int x);


            for (int i = y - step; i <= y + step; i++)
            {
                if ((i <= 8) && (i > 0))
                {

                    for (int j = x - step; j <= x + step; j++)
                    {
                        if ((j <= 8) && (j > 0))
                        {

                            string name = "T" + i.ToString() + j.ToString();

                            var myTextBlock = (TextBlock)this.FindName(name);
                            if (!field.Values.Contains(myTextBlock))
                            {
                                myTextBlock.Text = "\n * ";
                                myTextBlock.FontSize = 30;
                            }
                        }
                    }
                }
            }


        }

        private void Cleansteps()
        {
            for (int i = 1; i <= 8; i++)
            {

                for (int j = 1; j <= 8; j++)
                {
                    string name = "T" + i.ToString() + j.ToString();

                    var myTextBlock = (TextBlock)this.FindName(name);
                    myTextBlock.FontSize = 60;
                    if (!field.Values.Contains(myTextBlock))
                    {
                        myTextBlock.Text = " ";
                       
                    }
                }
            }

        }
        private void Move(object sender, MouseButtonEventArgs e)
        {

            var unit = battle.Whowillgo();
            TextBlock start = field[unit];

            TextBlock finish = (TextBlock)sender;

            if (field.ContainsValue(finish))
            {
                List<BattleUnitStack> a = field.Keys.ToList();
                for (int i = 0; i < field.Keys.Count; i++)
                {
                    if (field[a[i]] == finish)
                    {
                        if (unit.bus.Range >= Distance(start, finish))
                        {
                            var beatbef = a[i].bus.qty * a[i].bus.StandardHitpoints + a[i].bus.Hitpoints;
                            string s = battle.Attack(unit, a[i]);
                            TextTip(field[unit], unit);
                            TextTip(field[a[i]], a[i]);
                            if (s == "Same Army")
                            {

                                battleStatus.Text = "Same Army. Choose another action" + "\n";
                                return;
                            }
                            else if (s == "Damaged")
                            {
                                battleStatus.Text = finish.Text + " " + s + " by " + start.Text + "\n";
                                unit.canBeUse = false;
                                Endturn(unit);
                            }
                            if (s == "Killed")
                            {
                                field.Remove(a[i]);
                                TextTip(finish);
                                finish.Text = "";
                                finish.Foreground = Brushes.Black;
                                unit.canBeUse = false;
                                Endturn(unit);
                            }
                            if (s == "Was Failed by")
                            {
                                field.Remove(unit);
                                TextTip(start);
                                start.Text = "";
                                start.Foreground = Brushes.Black;
                                unit.canBeUse = false;
                                Endturn(unit);
                            }
                        }
                        else
                        {
                            battleStatus.Text = "Not enough Range skills to attack enemy unit" + "\n";
                            return;
                        }
                    }
                }
            }
            else 
            {
                if (unit.bus.Step >= Distance(start, finish))
                {
                    finish.Text = start.Text;
                    field.Remove(unit);
                    field.Add(unit, finish);
                    start.Text = "";
                    start.Foreground = Brushes.Black;
                    TextTip(start, finish, unit);
                    unit.canBeUse = false;
                    Endturn(unit);
                }
                else
                {
                    battleStatus.Text = "Not enough Step skills to change position as far as you want" + "\n";
                }
            }
        }
   
        private void WaitButton(object sender, RoutedEventArgs e)
        {

            var unit = battle.Whowillgo();
            battle.Wait(unit);
            ListOfSpells();
            Colorise(unit, field[unit]);

            Endturn(unit);
        }



        private void DefendButton(object sender, RoutedEventArgs e)
        {
            var unit = battle.Whowillgo();
            battle.Defend(unit);
            ListOfSpells();
            Endturn(unit);
        }

        private void DefeatButton(object sender, RoutedEventArgs e)
        {
            var unit = battle.Whowillgo();
            battle.Defeat(unit);
        }

        private void Chooseabil(object sender, SelectionChangedEventArgs e)
        {
            var unit = battle.Whowillgo();
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
        private void ListOfSpells()
        {
            spellsList.Clear();
            feat.Clear();
            var unit = battle.Whowillgo();
            feat.Add("\nDefault Spells (Clickable) :");
            if (unit != null)
            {
                foreach (var a in unit.abl)
                {
                    feat.Add(a.Name);
                    spellsList.Add(a.Name, a);
                }
                abilities.ItemsSource = feat;
            }
        }
    }

}
