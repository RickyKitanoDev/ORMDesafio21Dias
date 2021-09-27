using System;
using System.Collections.Generic;
using System.Text;

namespace ORMDesafio21Dias
{
    public abstract class CType : IType, IConnectionString
    {
        [Table(PrimaryKey = "id")]
        public virtual int Id { get; set; }
        [Table(IsNotOnDataBase = true)] 
        public abstract string ConnectionString { get;}

        public virtual void Save()
        {
            new Service(this).Save();
        }

        public virtual void Destroy()
        {
            new Service(this).Destroy();
        }

        public virtual void Get()
        {
            new Service(this).Get();
        }
    }
}
