using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using ATM.UserInterface;
using ATM.Classes;


namespace ATM
{
    class GameScene : Scene
    {

        public void ShowMainMenu()
        {
            Add(new Background(Assets.Background));
            Add(new Cursor());

            Add(new ATM.UserInterface.Button(Assets.Button, 300 - 10, 200,Assets.Button, delegate()
                {
                    GameScene.Instance.RemoveAll();
                    ShowCash();
                }));

            Add(new Label(310, 200, "Посмотреть баланс", Color.White, 16));

            Add(new ATM.UserInterface.Button(Assets.Button, 300 - 10, 300, Assets.Button, delegate()
            {
                    GameScene.Instance.RemoveAll();
                    GetCash();
            }));

            Add(new Label(325, 300, "Снять наличные", Color.White, 16));


            Add(new ATM.UserInterface.Button(Assets.Button, 300 - 10, 400, Assets.Button, delegate()
            {
                Global.atm.SaveATM();
                Environment.Exit(0);
            }));

            Add(new Label(350, 400, "Выход", Color.White, 16));

            Add(new Label(Game.Instance.HalfHeight, 20, "БАНКОМАТ", Color.White, 40));
        }

        public void ShowCash()
        {
            Add(new Background(Assets.Background));
            Add(new Cursor());
            double currentMoney = Global.atm.CurrentCardAvailableMoney();
            Add(new Label(Game.Instance.HalfWidth-100, Game.Instance.HalfHeight, "Ваш баланс - " + currentMoney, Color.Black));
            Add(new ATM.UserInterface.Button(Assets.Button, Game.HalfWidth - 100, Game.HalfHeight + 100, Assets.Button, delegate()
                {
                    GameScene.Instance.RemoveAll();
                    ShowMainMenu();
                }));
            Add(new Label(Game.HalfHeight+80, Game.HalfHeight + 110, "Назад", Color.Black));
        }

        public void GetCash()
        {
            Add(new Background(Assets.Background));
            Add(new Cursor());
            var sum = new TextEditBox(250, 200);
            Add(new Label(320, 20, "Снять деньги", Color.White, 30));
            Add(new Label(250, 180, "Номер карты", Color.Black));
            Add(new ATM.UserInterface.Button(Assets.Button, 330, 400, Assets.Button, delegate()
            {
                if (Global.atm.TakeMoney(Convert.ToInt32(sum.InputString)))
                {
                    GameScene.Instance.RemoveAll();
                    ShowResult(sum.InputString);
                }
                else
                {
                    GameScene.Instance.RemoveAll();
                    Error();
                }
            })); 
            Add(sum);
        }

        public void ShowResult(string sum)
        {
            Add(new Background(Assets.Background));
            Add(new Cursor());
            Add(new Label(340, 20, "Успех", Color.White, 30));
            Add(new Label(250, 180, "Деньги в размере " + sum + " были успешно сняты", Color.Black));
        }

        public void Error()
        {
            Add(new Background(Assets.Background));
            Add(new Cursor());
            Add(new Label(340, 20, "Ошибка", Color.White, 30));
            Add(new Label(250, 180, "На вашем счету не хватает средств", Color.Black));
        }

        public void ShowAuthorizeForm()
        {
            Add(new Background(Assets.Background));
            Add(new Cursor());
            var card = new TextEditBox(250, 200);
            var password = new TextEditBox(250, 300);
            Add(new Label(380, 20, "Вход", Color.White, 30));
            Add(card);
            Add(new Label(250, 180, "Номер карты", Color.Black));
            Add(password);
            Add(new Label(250, 280, "Пароль", Color.Black));
            Add(new ATM.UserInterface.Button(Assets.Button, 330, 400, Assets.Button, delegate()
                {
                    bool res = Global.atm.RunATM(card.InputString, password.InputString);
                    if (res)
                    {
                        GameScene.Instance.RemoveAll();
                        ShowMainMenu();
                    }
                    else
                    {
                        card.ImageBox.OutlineColor = Color.Red;
                        password.ImageBox.OutlineColor = Color.Red;
                        Add(new Label(280, 350, "Невалидные данные.Попробуйте ещё раз.", Color.Red));
                    }
                }));
            Add(new Label(380, 400, "Подтвердить", Color.White));
        }

        public GameScene()
        {
            ShowAuthorizeForm();
        }
    }
}
