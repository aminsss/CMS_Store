namespace Koshop.DataLayer
{
    using Koshop.DomainClasses;
    using System.Data.Entity;
    using Koshop.DataLayer.Mapping;

    public class AppDbContext : DbContext
    {
        // Your context has been configured to use a 'ApplicationDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Koshop.DataLayer.ApplicationDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ApplicationDbContext' 
        // connection string in the application configuration file.
        public AppDbContext()
            : base("name=AppDbContext")
        {
        }

        public virtual DbSet<Address_User> Address_Users { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<ContactModule> ContactModules { get; set; }
        public virtual DbSet<ContactPerson> ContactPersons { get; set; }
        public virtual DbSet<HtmlModule> HtmlModules { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuGroup> MenuGroups { get; set; }
        public virtual DbSet<MenuModule> MenuModules { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<ModulePage> ModulePages { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsGallery> NewsGalleries { get; set; }
        public virtual DbSet<NewsGroup> NewsGroups { get; set; }
        public virtual DbSet<NewsTag> NewsTags { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<ProductGallery> ProductGalleries { get; set; }
        public virtual DbSet<ProductGroup> ProductGroups { get; set; }
        public virtual DbSet<ProductTag> ProductTages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserStore> UserStores { get; set; }
        public virtual DbSet<AttributGrp> AttributGrps { get; set; }
        public virtual DbSet<AttributItem> AttributItems { get; set; }
        public virtual DbSet<chartPost> chartPosts { get; set; }
        public virtual DbSet<Product_Attribut> Product_Attributs { get; set; }
        public virtual DbSet<ProductRequest> ProductRequests { get; set; }
        public virtual DbSet<tbl_Help> tbl_Helps { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketGroup> TicketGroups { get; set; }
        public virtual DbSet<TicketMsg> TicketMsgs { get; set; }
        public virtual DbSet<StoreTime> StoreTimes { get; set; }
        public virtual DbSet<DetailItem> DetailItems { get; set; }
        public virtual DbSet<DetailGroup> DetailGroups { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<StoreInfo> StoreInfo { get; set; }
        public virtual DbSet<StoreFollower> StoreFollowers { get; set; }
        public virtual DbSet<StoresProduct> StoresProducts { get; set; }
        public virtual DbSet<RequestDetail> RequestDetails { get; set; }
        public virtual DbSet<MultiPictureModule> MultiPictureModules { get; set; }
        public virtual DbSet<MultiPictureItems> MultiPictureItems  { get; set; }
        public virtual DbSet<ComponentDesign> ComponentDesigns { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<BlockedIp> BlockedIps  { get; set; }
        public virtual DbSet<Statistics> Statistics  { get; set; }
        public virtual DbSet<Options> Options  { get; set; }


        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Configurations.Add(new Address_UserConfig());
            builder.Configurations.Add(new AttributGrpConfig());
            builder.Configurations.Add(new chartPostConfig());
            builder.Configurations.Add(new CityConfig());
            builder.Configurations.Add(new ComponentConfig());
            builder.Configurations.Add(new ContactModuleConfig());
            builder.Configurations.Add(new DetailItemConfig());
            builder.Configurations.Add(new StoreInfoConfig());
            builder.Configurations.Add(new ModuleConfig());
            builder.Configurations.Add(new UserConfig());



            base.OnModelCreating(builder);

           

        }

       

    }



    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}