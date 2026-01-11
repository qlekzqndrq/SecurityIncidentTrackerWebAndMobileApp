using CommunityToolkit.Maui.Views;

namespace SecurityIncidentTrackerMobileApp.Views
{
    public partial class ActionPopup : Popup
    {
        public ActionPopup(bool showDetails = false)
        {
            InitializeComponent();

            // Acum folosim lblDetails in loc de btnDetails
            lblDetails.IsVisible = showDetails;
        }

        private void OnEditClicked(object sender, EventArgs e) => Close("Edit");
        private void OnDetailsClicked(object sender, EventArgs e) => Close("Details");
        private void OnDeleteClicked(object sender, EventArgs e) => Close("Delete");
        private void OnCancelClicked(object sender, EventArgs e) => Close("Cancel");
    }
}