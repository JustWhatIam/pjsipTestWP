using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using pjsipTest.Resources;
using pjsipLibRuntimeComponent;

namespace pjsipTest {
    public partial class MainPage : PhoneApplicationPage {

        // 建構函式
        public MainPage() {
            InitializeComponent();
            // 將 ApplicationBar 當地語系化的程式碼範例
            //BuildLocalizedApplicationBar();
        }

        string candidateSDP = null;

        pjRC pj;
        private void initButton_Click(object sender, RoutedEventArgs e) {
            if (pj == null) {
                pj = new pjRC();
                pj.setDataHandler(ReceiveData);
            }
            pj.initSession((bool)IsOffererCheck.IsChecked ? 0 : 1);
        }

        private void showButton_Click(object sender, RoutedEventArgs e) {
            pj.show();
            statusText.Text = pj.GetLog();
        }

        private void startButton_Click(object sender, RoutedEventArgs e) {
            pj.start();
        }

        private void destroyButton_Click(object sender, RoutedEventArgs e) {
            if (pj != null)
                pj.destroy();
        }

        private void GetSdpListBtn_Click(object sender, RoutedEventArgs e) {
            JSONHelper.GetList(GetListEventHandler);
        }

        private void registerButton_Click(object sender, RoutedEventArgs e) {
            pj.show();
            string data = pj.GetLog();
            JSONHelper.Register(nameText.Text, data.Replace("\n", "\\n"));
        }

        private void GetListEventHandler(object obj, EventArgs args) {
            Dispatcher.BeginInvoke(() => {
                sdpList.DataContext = obj;
            });
        }

        private void inputButton_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(candidateSDP))
                MessageBox.Show("choose one sdp");
            else {
                System.Diagnostics.Debug.WriteLine(candidateSDP.Length);
                pj.inputSDP(candidateSDP);

            }
        }

        private void sdpList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(e.AddedItems.Count > 0)
                candidateSDP = (e.AddedItems[0] as SDP).Data;
        }

        private void sendButton_Click(object sender, RoutedEventArgs e) {
            if (pj != null) {
                pj.send(nameText.Text);
                
            }
        }

        private void ReceiveData(string data) {
            Dispatcher.BeginInvoke(() => {
                statusText.Text += data;
            });
        }

        

        

        // 建置當地語系化 ApplicationBar 的程式碼範例
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 將頁面的 ApplicationBar 設定為 ApplicationBar 的新執行個體。
        //    ApplicationBar = new ApplicationBar();

        //    // 建立新的按鈕並將文字值設定為 AppResources 的當地語系化字串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 用 AppResources 的當地語系化字串建立新的功能表項目。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}