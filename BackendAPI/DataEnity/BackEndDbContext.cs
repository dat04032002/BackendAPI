using Microsoft.EntityFrameworkCore;
using System;
using Core.Model;

namespace BackendAPI.DataEnity
{
    public class BackEndDbContext: DbContext
    {
        public BackEndDbContext(DbContextOptions<BackEndDbContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<UserTest> userTests { get; set; }
        public DbSet<UserTestDetail> userTestDetails { get; set; }
        public DbSet<CodeTest> codeTests { get; set; }
        public DbSet<MultipleChoiceTest> multipleChoiceTests { get; set; }
        public DbSet<DanhMucChiTiet> danhMucChiTiets { get; set; }
        public DbSet<Test> tests { get; set; }
        public DbSet<DanhMuc> danhMucs { get; set; }

    }
}
