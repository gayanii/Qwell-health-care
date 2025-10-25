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
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace QWellApp.ViewModels
{
    public class ProductRecordViewModel: ViewModelBase
    {
        private IEnumerable<ProductRecordView> _productRecordList;
        private string _noResults;
        private string _searchWord = "";
        public int _selectedId;
        private string _barcode;
        private DateTime _expDate = DateTime.Today;
        private string _supplier;
        private int _orderedQty;
        private string _product;
        private float _supplierPrice;
        private float _sellingPrice;
        private string _addedby;
        private DateTime _receivedDate = DateTime.Today;
        private Dictionary<int, string> _supplierList;
        private Dictionary<int, string> _productList;
        private Dictionary<int, string> _EmployeeList;
        private string _productNameErrorMessage;
        private string _supplierPriceErrorMessage;
        private string _sellingPriceErrorMessage;
        private string _orderedQtyErrorMessage;
        private string _receivedDateErrorMessage;
        private string _addedByNameErrorMessage;
        private string _supplierNameErrorMessage;
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
        private IProductRecordRepository productRecordRepository;
        private ISupplierRepository supplierRepository;
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

        public string Barcode
        {
            get
            {
                return _barcode;
            }

            set
            {
                _barcode = value;
                OnPropertyChanged(nameof(Barcode));
            }
        }

        public DateTime ExpDate
        {
            get
            {
                return _expDate;
            }

            set
            {
                _expDate = value;
                OnPropertyChanged(nameof(ExpDate));
            }
        }

        public string Supplier
        {
            get
            {
                return _supplier;
            }

            set
            {
                _supplier = value;
                OnPropertyChanged(nameof(Supplier));
            }
        }

        public int OrderedQty
        {
            get
            {
                return _orderedQty;
            }

            set
            {
                _orderedQty = value;
                OnPropertyChanged(nameof(OrderedQty));
            }
        }
        public string Product
        {
            get { return _product; }
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        public float SupplierPrice
        {
            get { return _supplierPrice; }
            set
            {
                _supplierPrice = value;
                OnPropertyChanged(nameof(SupplierPrice));
            }
        }

        public float SellingPrice
        {
            get { return _sellingPrice; }
            set
            {
                _sellingPrice = value;
                OnPropertyChanged(nameof(SellingPrice));
            }
        }

        public string AddedBy
        {
            get { return _addedby; }
            set
            {
                _addedby = value;
                OnPropertyChanged(nameof(AddedBy));
            }
        }

        public DateTime ReceivedDate
        {
            get { return _receivedDate; }
            set
            {
                _receivedDate = value;
                OnPropertyChanged(nameof(ReceivedDate));
            }
        }
        public string SupplierNameErrorMessage
        {
            get
            {
                return _supplierNameErrorMessage;
            }

            set
            {
                _supplierNameErrorMessage = value;
                OnPropertyChanged(nameof(SupplierNameErrorMessage));
            }
        }
        public string ProductNameErrorMessage
        {
            get { return _productNameErrorMessage; }
            set
            {
                _productNameErrorMessage = value;
                OnPropertyChanged(nameof(ProductNameErrorMessage));
            }
        }

        public string SupplierPriceErrorMessage
        {
            get { return _supplierPriceErrorMessage; }
            set
            {
                _supplierPriceErrorMessage = value;
                OnPropertyChanged(nameof(SupplierPriceErrorMessage));
            }
        }

        public string SellingPriceErrorMessage
        {
            get { return _sellingPriceErrorMessage; }
            set
            {
                _sellingPriceErrorMessage = value;
                OnPropertyChanged(nameof(SellingPriceErrorMessage));
            }
        }

        public string OrderedQtyErrorMessage
        {
            get { return _orderedQtyErrorMessage; }
            set
            {
                _orderedQtyErrorMessage = value;
                OnPropertyChanged(nameof(OrderedQtyErrorMessage));
            }
        }

        public string ReceivedDateErrorMessage
        {
            get { return _receivedDateErrorMessage; }
            set
            {
                _receivedDateErrorMessage = value;
                OnPropertyChanged(nameof(ReceivedDateErrorMessage));
            }
        }

        public string AddedByNameErrorMessage
        {
            get { return _addedByNameErrorMessage; }
            set
            {
                _addedByNameErrorMessage = value;
                OnPropertyChanged(nameof(AddedByNameErrorMessage));
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

        public IEnumerable<ProductRecordView> ProductRecordList
        {
            get
            {
                return _productRecordList;
            }
            set
            {
                _productRecordList = value;
                OnPropertyChanged(nameof(ProductRecordList));
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

        public Dictionary<int, string> SupplierList
        {
            get
            {
                return _supplierList;
            }
            set
            {
                _supplierList = value;
                OnPropertyChanged(nameof(SupplierList));
            }
        }
        public Dictionary<int, string> ProductList
        {
            get { return _productList; }
            set
            {
                _productList = value;
                OnPropertyChanged(nameof(ProductList));
            }
        }

        public Dictionary<int, string> EmployeeList
        {
            get { return _EmployeeList; }
            set
            {
                _EmployeeList = value;
                OnPropertyChanged(nameof(EmployeeList));
            }
        }

        //Commands
        public ICommand LoadSearchResults { get; }
        public ICommand UpdateProductRecordCommand { get; }
        public ICommand CreateProductRecordCommand { get; }
        public ICommand GetProductRecordDetails { get; }
        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand DeleteCommand { get; }

        public ProductRecordViewModel()
        {
            ProductRecordList = new List<ProductRecordView>();
            SupplierList = new Dictionary<int, string>();
            //ProductList = new Dictionary<int, string>();
            EmployeeList = new Dictionary<int, string>();
            productRepository = new ProductRepository();
            productRecordRepository = new ProductRecordRepository();
            userRepository = new UserRepository();
            supplierRepository = new SupplierRepository();
            LoadProductList(SearchWord);
            activityLogRepository = new ActivityLogRepository();
            currentUser = userRepository.GetByUsername(Properties.Settings.Default.Username);
            LoadSearchResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAllUsersCommand);
            GetProductRecordDetails = new RelayCommand(ExecuteGetProductRecordDetailsCommand, CanExecuteGetProductRecordDetailsCommand);
            UpdateProductRecordCommand = new RelayCommand(ExecuteUpdateCommand, CanExecuteForAdminsCommand);
            CreateProductRecordCommand = new RelayCommand(ExecuteCreateCommand, CanExecuteForAdminsCommand);
            ResetCommand = new RelayCommand(ExecuteGetProductRecordDetailsCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteUserCommand);
            BackToListCommand = new RelayCommand(ExecuteBackToListCommand);
            ButtonVisibility();
            LoadSupplierList();
            LoadProductList();
            LoadEmployeeList();
        }

        private void LoadSupplierList()
        {
            Dictionary<int, string> suppliers = new Dictionary<int, string>();
            var supplierList = supplierRepository.GetAll("");
            foreach (var supplier in supplierList)
            {
                suppliers.Add(supplier.Id, supplier.CompanyName);
            }
            SupplierList = suppliers;
        }
        private void LoadProductList()
        {
            Dictionary<int, string> products = new Dictionary<int, string>();
            var productList = productRepository.GetAll("");
            foreach (var product in productList)
            {
                products.Add(product.Id, product.BrandName + " - " + product.Generic);
            }
            ProductList = products;
        }
        private void LoadEmployeeList()
        {
            Dictionary<int, string> employees = new Dictionary<int, string>();
            var employeeList = userRepository.GetAll("");
            foreach (var employee in employeeList)
            {
                employees.Add(employee.Id, employee.FirstName + " " + employee.LastName);
            }
            EmployeeList = employees;
        }
        private void ExecuteDeleteCommand(object obj)
        {
            var oldData = productRecordRepository.GetByID(SelectedId);
            var deleteSuccess = productRecordRepository.Remove(SelectedId);
            if (deleteSuccess)
            {
                // Log the activity
                var log = new ActivityLog
                {
                    AffectedEntity = EntitiesEnum.ProductRecords,
                    AffectedEntityId = SelectedId,
                    ActionType = ActionTypeEnum.Delete,
                    OldValues = JsonConvert.SerializeObject(new
                    {
                        oldData.Id,
                        oldData.Barcode,
                        oldData.ProductId,
                        Product = (object?)null,
                        oldData.SupplierPrice,
                        oldData.SellingPrice,
                        oldData.OrderedQuantity,
                        oldData.ExpDate,
                        oldData.ReceivedDate,
                        oldData.SupplierId,
                        Supplier = (object?)null,
                        oldData.UserId,
                        User = (object?)null,
                    }),
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
            Barcode = string.Empty;
            ExpDate = DateTime.Now;
            Supplier = string.Empty;
            OrderedQty = 0;
            Product = string.Empty;
            SupplierPrice = 0;
            SellingPrice = 0;
            AddedBy = string.Empty;
            ReceivedDate = DateTime.Now;

            //clear error msgs
            ProductNameErrorMessage = "";
            SupplierPriceErrorMessage = "";
            SellingPriceErrorMessage = "";
            OrderedQtyErrorMessage = "";
            ReceivedDateErrorMessage = "";
            AddedByNameErrorMessage = "";
            SupplierNameErrorMessage = "";
        }

        private void ExecuteCreateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(Product) || string.IsNullOrWhiteSpace(SupplierPrice.ToString()) || string.IsNullOrWhiteSpace(SellingPrice.ToString()) ||
                string.IsNullOrWhiteSpace(OrderedQty.ToString()) ||
                string.IsNullOrWhiteSpace(ReceivedDate.ToString()) || string.IsNullOrWhiteSpace(Supplier))
            {
                ProductNameErrorMessage = (string.IsNullOrWhiteSpace(Product)) ? "Product name is required." : "";
                SupplierPriceErrorMessage = (string.IsNullOrWhiteSpace(SupplierPrice.ToString())) ? "Supplier price is required." : "";
                SellingPriceErrorMessage = (string.IsNullOrWhiteSpace(SellingPrice.ToString())) ? "Selling price is required." : "";
                OrderedQtyErrorMessage = (string.IsNullOrWhiteSpace(OrderedQty.ToString())) ? "Ordered quantity is required." : "";
                AddedByNameErrorMessage = (string.IsNullOrWhiteSpace(AddedBy)) ? "Added by name is required." : "";
                ReceivedDateErrorMessage = (string.IsNullOrWhiteSpace(ReceivedDate.ToString())) ? "Received date is required." : "";
                SupplierNameErrorMessage = (string.IsNullOrWhiteSpace(Supplier)) ? "Supplier name is required." : "";
            }
            else
            {
                if (!Application.Current.Properties.Contains("Username"))
                {
                    return;
                }
                string username = (string)Application.Current.Properties["Username"];
                var user = userRepository.GetByUsername(username);
                ProductRecord createProductRecord = new ProductRecord()
                {
                    Barcode = Barcode,
                    ExpDate = ExpDate,
                    SupplierId = SupplierList.FirstOrDefault(x => x.Value == Supplier).Key,
                    OrderedQuantity = OrderedQty,
                    ProductId = ProductList.FirstOrDefault(x => x.Value == Product).Key,
                    SupplierPrice = SupplierPrice,
                    SellingPrice = SellingPrice,
                    UserId = user.Id,
                    ReceivedDate = DateTime.Now
                };
                var createSuccess = productRecordRepository.Add(createProductRecord);
                if (createSuccess)
                {
                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.ProductRecords,
                        AffectedEntityId = createProductRecord.Id,
                        ActionType = ActionTypeEnum.Add,
                        OldValues = "-",
                        NewValues = JsonConvert.SerializeObject(createProductRecord)
                    };
                    activityLogRepository.AddLog(log, currentUser);
                    UpdateGridVisibility = false;
                    ProductListVisibility = true;
                    CreateGridVisibility = false;
                    LoadProductList("");
                }
            }
        }

        private bool CanExecuteGetProductRecordDetailsCommand(object obj)
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

        private void ExecuteGetProductRecordDetailsCommand(object obj)
        {
            ProductRecord product = productRecordRepository.GetByID(SelectedId);
            Barcode = product.Barcode;
            ExpDate = (DateTime)product.ExpDate;
            Supplier = SupplierList[product.SupplierId];
            OrderedQty = product.OrderedQuantity;
            Product = ProductList[product.ProductId];
            SupplierPrice = product.SupplierPrice;
            SellingPrice = product.SellingPrice;
            AddedBy = EmployeeList[product.UserId];
            ReceivedDate = product.ReceivedDate;
        }

        private void ExecuteUpdateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(Product) || string.IsNullOrWhiteSpace(SupplierPrice.ToString()) || string.IsNullOrWhiteSpace(SellingPrice.ToString()) ||
                string.IsNullOrWhiteSpace(OrderedQty.ToString()) ||
                string.IsNullOrWhiteSpace(ReceivedDate.ToString()) || string.IsNullOrWhiteSpace(Supplier))
            {
                ProductNameErrorMessage = (string.IsNullOrWhiteSpace(Product)) ? "Product name is required." : "";
                SupplierPriceErrorMessage = (string.IsNullOrWhiteSpace(SupplierPrice.ToString())) ? "Supplier price is required." : "";
                SellingPriceErrorMessage = (string.IsNullOrWhiteSpace(SellingPrice.ToString())) ? "Selling price is required." : "";
                OrderedQtyErrorMessage = (string.IsNullOrWhiteSpace(OrderedQty.ToString())) ? "Ordered quantity is required." : "";
                AddedByNameErrorMessage = (string.IsNullOrWhiteSpace(AddedBy)) ? "Added by name is required." : "";
                ReceivedDateErrorMessage = (string.IsNullOrWhiteSpace(ReceivedDate.ToString())) ? "Received date is required." : "";
                SupplierNameErrorMessage = (string.IsNullOrWhiteSpace(Supplier)) ? "Supplier name is required." : "";
            }
            else
            {
                if (!Application.Current.Properties.Contains("Username"))
                {
                    return;
                }
                string username = (string)Application.Current.Properties["Username"];
                var user = userRepository.GetByUsername(username);
                ProductRecord updateProductRecord = new ProductRecord()
                {
                    Id = SelectedId,
                    Barcode = (Barcode == "") ? null : Barcode,
                    ExpDate = ExpDate,
                    SupplierId = SupplierList.FirstOrDefault(x => x.Value == Supplier).Key,
                    OrderedQuantity = OrderedQty,
                    ProductId = ProductList.FirstOrDefault(x => x.Value == Product).Key,
                    SupplierPrice = SupplierPrice,
                    SellingPrice = SellingPrice,
                    UserId = user.Id,
                    ReceivedDate = ReceivedDate
                };
                var oldData = productRecordRepository.GetByID(updateProductRecord.Id);
                bool editSuccess = productRecordRepository.Edit(updateProductRecord);
                if (editSuccess)
                {
                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.ProductRecords,
                        AffectedEntityId = updateProductRecord.Id,
                        ActionType = ActionTypeEnum.Update,
                        //OldValues = JsonConvert.SerializeObject(oldData), // Serialize the whole object
                        OldValues = JsonConvert.SerializeObject(new
                        {
                            oldData.Id,
                            oldData.Barcode,
                            oldData.ProductId,
                            Product = (object?)null,
                            oldData.SupplierPrice,
                            oldData.SellingPrice,
                            oldData.OrderedQuantity,
                            oldData.ExpDate,
                            oldData.ReceivedDate,
                            oldData.SupplierId,
                            Supplier = (object?)null,
                            oldData.UserId,
                            User = (object?)null,
                        }),
                        NewValues = JsonConvert.SerializeObject(updateProductRecord) // Serialize the whole object
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
            var products = productRecordRepository.GetAll(searchWord);
            ProductRecordList = products;
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
