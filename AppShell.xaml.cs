using E_Commercenaveen.Pages;

namespace E_Commercenaveen
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
        }
    }
}
