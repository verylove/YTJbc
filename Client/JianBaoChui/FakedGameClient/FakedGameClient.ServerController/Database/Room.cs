//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FakedGameClient.ServerController.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Room
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Point { get; set; }
        public string Ip { get; set; }
        public string Remark { get; set; }
        public Nullable<long> RoomLvId { get; set; }
        public Nullable<long> CurPoint { get; set; }
    }
}
