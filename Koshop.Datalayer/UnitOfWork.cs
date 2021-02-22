using System;
using Koshop.DataLayer;
using Koshop.DomainClasses;

namespace Koshop.DataLayer
{
    public class UnitOfWork : IDisposable
    {
        AppDbContext db = new AppDbContext();

        private Repository<User> _userRepository;
        public Repository<User> UserRepository
        {
            get { return _userRepository ?? (_userRepository = new Repository<User>(db));}
        }


        private Repository<Role> _roleRepository;
        public Repository<Role> RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new Repository<Role>(db)); }
        }

        private Repository<Product> _productRepository;
        public Repository<Product> ProductRepository
        {
            get { return _productRepository ?? (_productRepository = new Repository<Product>(db)); }
        }

        private Repository<ProductGroup> _productGroupRepository;
        public Repository<ProductGroup> ProductGroupRepository
        {
            get { return _productGroupRepository ?? (_productGroupRepository = new Repository<ProductGroup>(db)); }
        }

        private Repository<ProductGallery> _productGalleryRepository;
        public Repository<ProductGallery> ProductGalleryRepository
        {
            get { return _productGalleryRepository ?? (_productGalleryRepository = new Repository<ProductGallery>(db)); }
        }

        private Repository<ProductTag> _productTagRepository;
        public Repository<ProductTag> ProductTagRepository
        {
            get { return _productTagRepository ?? (_productTagRepository = new Repository<ProductTag>(db)); }
        }

        private Repository<Product_Attribut> _product_AttributRepository;
        public Repository<Product_Attribut> Product_AttributRepository
        {
            get { return _product_AttributRepository ?? (_product_AttributRepository = new Repository<Product_Attribut>(db)); }
        }

        private Repository<ProductDetail> _productDetailRepository;
        public Repository<ProductDetail> ProductDetailRepository
        {
            get { return _productDetailRepository ?? (_productDetailRepository = new Repository<ProductDetail>(db)); }
        }

        private Repository<AttributGrp> _attributGrpRepository;
        public Repository<AttributGrp> AttributGrpRepository
        {
            get { return _attributGrpRepository ?? (_attributGrpRepository = new Repository<AttributGrp>(db)); }
        }

        private Repository<AttributItem> _attributItemRepository;
        public Repository<AttributItem> AttributItemRepository
        {
            get { return _attributItemRepository ?? (_attributItemRepository = new Repository<AttributItem>(db)); }
        }


        private Repository<DetailItem> _detailItemRepository;
        public Repository<DetailItem> DetailItemRepository
        {
            get { return _detailItemRepository ?? (_detailItemRepository = new Repository<DetailItem>(db)); }
        }

        private Repository<DetailGroup> _detailGroupRepository;
        public Repository<DetailGroup> DetailGroupRepository
        {
            get { return _detailGroupRepository ?? (_detailGroupRepository = new Repository<DetailGroup>(db)); }
        }

        private Repository<MenuGroup> _menuGroupRepository;
        public Repository<MenuGroup> MenuGroupRepository
        {
            get { return _menuGroupRepository ?? (_menuGroupRepository = new Repository<MenuGroup>(db)); }
        }

        private Repository<Menu> _menuRepository;
        public Repository<Menu> MenuRepository
        {
            get { return _menuRepository ?? (_menuRepository = new Repository<Menu>(db)); }
        }

        private Repository<NewsGroup> _newsGroupRepository;
        public Repository<NewsGroup> NewsGroupRepository
        {
            get { return _newsGroupRepository ?? (_newsGroupRepository = new Repository<NewsGroup>(db)); }
        }

        private Repository<News> _newsRepository;
        public Repository<News> NewsRepository
        {
            get { return _newsRepository ?? (_newsRepository = new Repository<News>(db)); }
        }

        private Repository<NewsTag> _newsTagRepository;
        public Repository<NewsTag> NewsTagRepository
        {
            get { return _newsTagRepository ?? (_newsTagRepository = new Repository<NewsTag>(db)); }
        }

        private Repository<NewsComment> _newsCommentRepository;
        public Repository<NewsComment> NewsCommentRepository
        {
            get { return _newsCommentRepository ?? (_newsCommentRepository = new Repository<NewsComment>(db)); }
        }

        private Repository<NewsGallery> _newsGalleryRepository;
        public Repository<NewsGallery> NewsGalleryRepository
        {
            get { return _newsGalleryRepository ?? (_newsGalleryRepository = new Repository<NewsGallery>(db)); }
        }


        private Repository<Message> _messageRepository;
        public Repository<Message> MessageRepository
        {
            get { return _messageRepository ?? (_messageRepository = new Repository<Message>(db)); }
        }

        private Repository<Order> _orderRepository;
        public Repository<Order> OrderRepository
        {
            get { return _orderRepository ?? (_orderRepository = new Repository<Order>(db)); }
        }

        private Repository<OrderDetail> _orderDetailRepository;
        public Repository<OrderDetail> OrderDetailRepository
        {
            get { return _orderDetailRepository ?? (_orderDetailRepository = new Repository<OrderDetail>(db)); }
        }

        private Repository<Module> _moduleRepository;
        public Repository<Module> ModuleRepository
        {
            get { return _moduleRepository ?? (_moduleRepository = new Repository<Module>(db)); }
        }

        private Repository<ModulePage> _modulePageRepository;
        public Repository<ModulePage> ModulePageRepository
        {
            get { return _modulePageRepository ?? (_modulePageRepository = new Repository<ModulePage>(db)); }
        }

        private Repository<Position> _positionRepository;
        public Repository<Position> PositionRepository
        {
            get { return _positionRepository ?? (_positionRepository = new Repository<Position>(db)); }
        }

        private Repository<Component> _componentRepository;
        public Repository<Component> ComponentRepository
        {
            get { return _componentRepository ?? (_componentRepository = new Repository<Component>(db)); }
        }

        private Repository<MenuModule> _menuModuleRepository;
        public Repository<MenuModule> MenuModuleRepository
        {
            get { return _menuModuleRepository ?? (_menuModuleRepository = new Repository<MenuModule>(db)); }
        }

        private Repository<HtmlModule> _htmlModuleRepository;
        public Repository<HtmlModule> HtmlModuleRepository
        {
            get { return _htmlModuleRepository ?? (_htmlModuleRepository = new Repository<HtmlModule>(db)); }
        }

        private Repository<ContactModule> _contactModuleRepository;
        public Repository<ContactModule> ContactModuleRepository
        {
            get { return _contactModuleRepository ?? (_contactModuleRepository = new Repository<ContactModule>(db)); }
        }

        private Repository<ContactPerson> _contactPersonRepository;
        public Repository<ContactPerson> ContactPersonRepository
        {
            get { return _contactPersonRepository ?? (_contactPersonRepository = new Repository<ContactPerson>(db)); }
        }

        private Repository<MultiPictureModule> _multiPictureModuleRepository;
        public Repository<MultiPictureModule> MultiPictureModuleRepository
        {
            get { return _multiPictureModuleRepository ?? (_multiPictureModuleRepository = new Repository<MultiPictureModule>(db)); }
        }

        private Repository<MultiPictureItems> _multiPictureItemsRepository;
        public Repository<MultiPictureItems> MultiPictureItemsRepository
        {
            get { return _multiPictureItemsRepository ?? (_multiPictureItemsRepository = new Repository<MultiPictureItems>(db)); }
        }

        private Repository<BlockedIp> _blockedIpRepository;
        public Repository<BlockedIp> BlockedIpRepository
        {
            get { return _blockedIpRepository ?? (_blockedIpRepository = new Repository<BlockedIp>(db)); }
        }

        private Repository<Statistics> _statisticsRepository;
        public Repository<Statistics> StatisticsRepository
        {
            get { return _statisticsRepository ?? (_statisticsRepository = new Repository<Statistics>(db)); }
        }

        private Repository<Country> _countryRepository;
        public Repository<Country> CountryRepository
        {
            get { return _countryRepository ?? (_countryRepository = new Repository<Country>(db)); }
        }

        private Repository<Options> _optionsRepository;
        public Repository<Options> OptionsRepository
        {
            get { return _optionsRepository ?? (_optionsRepository = new Repository<Options>(db)); }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
