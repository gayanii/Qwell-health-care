using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using QWellApp.Enums;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace QWellApp.ViewModels
{
    public class ProductViewModel: ViewModelBase
    {
        private IEnumerable<ProductView> _productList;
        private string _noResults;
        private string _searchWord = "";
        public int _selectedId;
        private string _brandName;
        private string _generic;
        private int _currentQty;
        private float _sellingPrice;
        private string _status;
        private IEnumerable<string> _statusList;
        private string _brandNameErrorMessage;
        private string _genericErrorMessage;
        private string _currentQtyErrorMessage;
        private string _sellingPriceErrorMessage;
        private ViewModelBase _currentChildView;
        private bool _updateButtonVisibility = false;
        private bool _createButtonVisibility = false;
        private bool _deleteButtonVisibility = false;
        private bool _resetUpdateButtonsVisibility = false;
        private bool _updateGridVisibility = false;
        private bool _createGridVisibility = false;
        private bool _productListVisibility = true;

        private IUserRepository userRepository;
        private IProductRepository productRepository;
        private IActivityLogRepository activityLogRepository;
        private UserDetails currentUser;

        //Properties 
        public int SelectedId
        {
            get
            {
                return _selectedId;
            }

            set
            {
                _selectedId = value;
                OnPropertyChanged(nameof(SelectedId));
            }
        }

        public string BrandName
        {
            get
            {
                return _brandName;
            }

            set
            {
                _brandName = value;
                OnPropertyChanged(nameof(BrandName));
            }
        }

        public string Generic
        {
            get
            {
                return _generic;
            }

            set
            {
                _generic = value;
                OnPropertyChanged(nameof(Generic));
            }
        }
        public int CurrentQty
        {
            get
            {
                return _currentQty;
            }
            set
            {
                _currentQty = value;
                OnPropertyChanged(nameof(CurrentQty));
            }
        }

        public float SellingPrice
        {
            get
            {
                return _sellingPrice;
            }
            set
            {
                _sellingPrice = value;
                OnPropertyChanged(nameof(SellingPrice));
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public IEnumerable<string> StatusList
        {
            get
            {
                return _statusList;
            }
            set
            {
                _statusList = value;
                OnPropertyChanged(nameof(StatusList));
            }
        }

        public string GenericErrorMessage
        {
            get
            {
                return _genericErrorMessage;
            }
            set
            {
                _genericErrorMessage = value;
                OnPropertyChanged(nameof(GenericErrorMessage));
            }
        }

        public string CurrentQtyErrorMessage
        {
            get
            {
                return _currentQtyErrorMessage;
            }
            set
            {
                _currentQtyErrorMessage = value;
                OnPropertyChanged(nameof(CurrentQtyErrorMessage));
            }
        }

        public string SellingPriceErrorMessage
        {
            get
            {
                return _sellingPriceErrorMessage;
            }
            set
            {
                _sellingPriceErrorMessage = value;
                OnPropertyChanged(nameof(SellingPriceErrorMessage));
            }
        }
        public string BrandNameErrorMessage
        {
            get
            {
                return _brandNameErrorMessage;
            }

            set
            {
                _brandNameErrorMessage = value;
                OnPropertyChanged(nameof(BrandNameErrorMessage));
            }
        }
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public bool UpdateButtonVisibility
        {
            get
            {
                return _updateButtonVisibility;
            }

            set
            {
                _updateButtonVisibility = value;
                OnPropertyChanged(nameof(UpdateButtonVisibility));
            }
        }
        public bool CreateButtonVisibility
        {
            get
            {
                return _createButtonVisibility;
            }

            set
            {
                _createButtonVisibility = value;
                OnPropertyChanged(nameof(CreateButtonVisibility));
            }
        }
        public bool DeleteButtonVisibility
        {
            get
            {
                return _deleteButtonVisibility;
            }

            set
            {
                _deleteButtonVisibility = value;
                OnPropertyChanged(nameof(DeleteButtonVisibility));
            }
        }
        public bool ResetUpdateButtonsVisibility
        {
            get
            {
                return _resetUpdateButtonsVisibility;
            }

            set
            {
                _resetUpdateButtonsVisibility = value;
                OnPropertyChanged(nameof(ResetUpdateButtonsVisibility));
            }
        }
        public bool UpdateGridVisibility
        {
            get
            {
                return _updateGridVisibility;
            }

            set
            {
                _updateGridVisibility = value;
                OnPropertyChanged(nameof(UpdateGridVisibility));
            }
        }
        public bool CreateGridVisibility
        {
            get
            {
                return _createGridVisibility;
            }

            set
            {
                _createGridVisibility = value;
                OnPropertyChanged(nameof(CreateGridVisibility));
            }
        }
        public bool ProductListVisibility
        {
            get
            {
                return _productListVisibility;
            }

            set
            {
                _productListVisibility = value;
                OnPropertyChanged(nameof(ProductListVisibility));
            }
        }

        public IEnumerable<ProductView> ProductList
        {
            get
            {
                return _productList;
            }
            set
            {
                _productList = value;
                OnPropertyChanged(nameof(ProductList));
            }
        }

        public string NoResults
        {
            get 
            { 
                return _noResults; 
            }
            set
            {
                _noResults = value;
                OnPropertyChanged(nameof(NoResults));
            }
        }
        public string SearchWord
        {
            get
            {
                return _searchWord;
            }

            set
            {
                _searchWord = value;
                OnPropertyChanged(nameof(SearchWord));
            }
        }

        //Commands
        public ICommand LoadSearchResults { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand CreateProductCommand { get; }
        public ICommand GetProductDetails { get; }
        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand DeleteCommand { get; }

        public ProductViewModel()
        {
            ProductList = new List<ProductView>();
            productRepository = new ProductRepository();
            userRepository = new UserRepository();
            activityLogRepository = new ActivityLogRepository();
            currentUser = userRepository.GetByUsername(Properties.Settings.Default.Username);
            StatusList = new List<string>() { UserStatusEnum.Active.ToString(), UserStatusEnum.Inactive.ToString() };
            LoadProductList(SearchWord);
            LoadSearchResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAllUsersCommand);
            GetProductDetails = new RelayCommand(ExecuteGetProductDetailsCommand, CanExecuteGetProductDetailsCommand);
            UpdateProductCommand = new RelayCommand(ExecuteUpdateCommand, CanExecuteForAdminsCommand);
            CreateProductCommand = new RelayCommand(ExecuteCreateCommand, CanExecuteForAdminsCommand);
            ResetCommand = new RelayCommand(ExecuteGetProductDetailsCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteUserCommand);
            BackToListCommand = new RelayCommand(ExecuteBackToListCommand);
            ButtonVisibility();
        }

        private void ExecuteDeleteCommand(object obj)
        {
            var oldData = productRepository.GetByID(SelectedId);
            var deleteSuccess = productRepository.Remove(SelectedId);
            if (deleteSuccess)
            {
                // Log the activity
                var log = new ActivityLog
                {
                    AffectedEntity = EntitiesEnum.Products,
                    AffectedEntityId = SelectedId,
                    ActionType = ActionTypeEnum.Delete,
                    OldValues = JsonConvert.SerializeObject(oldData), // Serialize the whole object
                    NewValues = "-"
                };
                activityLogRepository.AddLog(log, currentUser);
                LoadProductList("");
            }
        }

        private void ExecuteBackToListCommand(object obj)
        {
            UpdateGridVisibility = false;
            ProductListVisibility = true;
            CreateGridVisibility = false;
            LoadProductList("");
        }

        private void ExecuteCancelCommand(object obj)
        {
            BrandName = string.Empty;
            Generic = string.Empty;
            CurrentQty = 0;
            SellingPrice = 0;
            Status = string.Empty;

            //clear error msgs
            BrandNameErrorMessage = "";
            GenericErrorMessage = "";
            CurrentQtyErrorMessage = "";
            SellingPriceErrorMessage = "";
        }

        private void ExecuteCreateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(BrandName) || string.IsNullOrWhiteSpace(Generic) || string.IsNullOrWhiteSpace(CurrentQty.ToString()) ||
                string.IsNullOrWhiteSpace(SellingPrice.ToString()))
            {
                BrandNameErrorMessage = (string.IsNullOrWhiteSpace(BrandName)) ? "Brand name is required." : "";
                GenericErrorMessage = (string.IsNullOrWhiteSpace(Generic)) ? "Generic is required." : "";
                CurrentQtyErrorMessage = (string.IsNullOrWhiteSpace(CurrentQty.ToString())) ? "Current quantity is required." : "";
                SellingPriceErrorMessage = (string.IsNullOrWhiteSpace(SellingPrice.ToString())) ? "Selling price is required." : "";
            }
            else
            {
                Product createProduct = new Product()
                {
                    BrandName = BrandName,
                    Generic = Generic,
                    CurrentQuantity = CurrentQty,
                    SellingPrice = SellingPrice
                };
                var createSuccess = productRepository.Add(createProduct);
                if (createSuccess)
                {
                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.Products,
                        AffectedEntityId = createProduct.Id,
                        ActionType = ActionTypeEnum.Add,
                        OldValues = "-",
                        NewValues = JsonConvert.SerializeObject(createProduct)
                    };
                    activityLogRepository.AddLog(log, currentUser);


                    UpdateGridVisibility = false;
                    ProductListVisibility = true;
                    CreateGridVisibility = false;
                    LoadProductList("");
                }
            }
        }

        private bool CanExecuteGetProductDetailsCommand(object obj)
        {
            if (SelectedId > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanExecuteDeleteUserCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null
                && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()))
                && validStatus.Equals(UserStatusEnum.Active.ToString()) && SelectedId > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExecuteGetProductDetailsCommand(object obj)
        {
            Product product = productRepository.GetByID(SelectedId);
            BrandName = product.BrandName;
            Generic = product.Generic;
            CurrentQty = product.CurrentQuantity;
            SellingPrice = product.SellingPrice;
            Status = product.Status;
        }

        private void ExecuteUpdateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(BrandName) || string.IsNullOrWhiteSpace(Generic) || string.IsNullOrWhiteSpace(CurrentQty.ToString()) ||
                string.IsNullOrWhiteSpace(SellingPrice.ToString()))
            {
                BrandNameErrorMessage = (string.IsNullOrWhiteSpace(BrandName)) ? "Brand name is required." : "";
                GenericErrorMessage = (string.IsNullOrWhiteSpace(Generic)) ? "Generic is required." : "";
                CurrentQtyErrorMessage = (string.IsNullOrWhiteSpace(CurrentQty.ToString())) ? "Current quantity is required." : "";
                SellingPriceErrorMessage = (string.IsNullOrWhiteSpace(SellingPrice.ToString())) ? "Selling price is required." : "";
            }
            else
            {
                Product updateProduct = new Product()
                {
                    Id = SelectedId,
                    BrandName = BrandName,
                    Generic = Generic,
                    CurrentQuantity = CurrentQty,
                    SellingPrice = SellingPrice,
                    Status = (Status == "") ? UserStatusEnum.Active.ToString() : Status,
                };
                var oldData = productRepository.GetByID(updateProduct.Id);
                bool editSuccess = productRepository.Edit(updateProduct);
                if (editSuccess)
                {
                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.Products,
                        AffectedEntityId = updateProduct.Id,
                        ActionType = ActionTypeEnum.Update,
                        OldValues = JsonConvert.SerializeObject(oldData), // Serialize the whole object
                        NewValues = JsonConvert.SerializeObject(updateProduct) // Serialize the whole object
                    };
                    activityLogRepository.AddLog(log, currentUser);
                    UpdateGridVisibility = false;
                    ProductListVisibility = true;
                    CreateGridVisibility = false;
                    LoadProductList("");
                }
            }
        }

        private bool CanExecuteForAdminsCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ButtonVisibility()
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                UpdateButtonVisibility = true;
                CreateButtonVisibility = true;
                DeleteButtonVisibility = true;
            }
            else if (valideUser != null && validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString()) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                UpdateButtonVisibility = true;
                CreateButtonVisibility = true;
                DeleteButtonVisibility = false;
            }
            else
            {
                UpdateButtonVisibility = false;
                CreateButtonVisibility = false;
                DeleteButtonVisibility = false;
            }
        }

        private void ExecuteSearchCommand(object obj)
        {
            LoadProductList(SearchWord);
        }

        private bool CanExecuteForAllUsersCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;

            if (valideUser != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void LoadProductList(string searchWord)
        {
            var products = productRepository.GetAll(searchWord);
            ProductList = products;
            if (products.Any())
            {
                NoResults = "Hidden";
            }
            else
            {
                NoResults = "Visible";
            }
        }
    }
}
