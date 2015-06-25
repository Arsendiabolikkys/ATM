using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using ATM.UserInterface;

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

            Add(new Label(Game.Instance.HalfHeight, 20, "БАНКОМАТ", Color.White, 40));
        }

        public void ShowCash()
        {
            //::TODO
        }

        public void GetCash()
        {
            //::TODO
        }

        public void ShowAuthorizeForm()
        {
            Add(new Background(Assets.Background));
            Add(new Cursor());
            Add(new Label(380, 20, "Вход", Color.White, 30));
            Add(new TextEditBox(250, 200));
            Add(new Label(250, 180, "Номер карты", Color.Black));
            Add(new TextEditBox(250, 300));
            Add(new Label(250, 280, "Пароль", Color.Black));
            Add(new ATM.UserInterface.Button(Assets.Button, 330, 400, Assets.Button, delegate()
                {
                    //::TODO
                }));
            Add(new Label(380, 400, "Подтвердить", Color.White));
        }

        public GameScene()
        {
            ShowAuthorizeForm();
        }
    }
}
