using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace LibraryCatalog
{
    public class Checkout
    {
        private int _id;
        private int _patronsId;
        private int _copiesId;
        private int _returned;
        private string _checkoutDate;
        private int _overdue;

        public Checkout(int patronsId, int copiesId, string checkoutDate, int returned = 0, int overdue = 0, int id = 0)
        {
            _id = id;
            _patronsId = patronsId;
            _copiesId = copiesId;
            _returned = returned;
            _checkoutDate = checkoutDate;
            _overdue = overdue;
        }

        public override bool Equals(System.Object otherCheckout)
        {
            if(!(otherCheckout is Checkout))
            {
                return false;
            }
            else
            {
                Checkout newCheckout = (Checkout) otherCheckout;
                bool idEquality = this.GetId() == newCheckout.GetId();
                bool patronsIdEquality = this.GetPatronsId() == newCheckout.GetPatronsId();
                bool copiesIdEquality = this.GetCopiesId() == newCheckout.GetCopiesId();
                bool returnedEquality = this.GetReturned() == newCheckout.GetReturned();
                bool checkoutDateEquality = this.GetCheckoutDate() == newCheckout.GetCheckoutDate();
                bool overdueEquality = this.GetOverdue() == newCheckout.GetOverdue();

                return(idEquality && patronsIdEquality && copiesIdEquality && returnedEquality && checkoutDateEquality && overdueEquality);
            }
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (patrons_id, copies_id, checkout_date, returned, overdue) OUTPUT INSERTED.id VALUES (@PatronsId, @CopiesId, @Returned, @CheckoutDate, @Overdue);", conn);

            cmd.Parameters.Add(new SqlParameter("@PatronsId", this.GetPatronsId()));
            cmd.Parameters.Add(new SqlParameter("@CopiesId", this.GetCopiesId()));
            cmd.Parameters.Add(new SqlParameter("@Returned", this.GetReturned()));
            cmd.Parameters.Add(new SqlParameter("@CheckoutDate", this.GetCheckoutDate()));
            cmd.Parameters.Add(new SqlParameter("@Overdue", this.GetOverdue()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            DB.CloseSqlConnection(rdr, conn);
        }

        public static List<Checkout> FindCheckoutByPatronId(int id)
        {
            List<Checkout> patronsCheckouts = new List<Checkout>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM checkotus WHERE patrons_id = @PatronsId;", conn);

            cmd.Parameters.Add(new SqlParameter("@PatronsId", id));

            SqlDataReader rdr = cmd.ExecuteReader();


            while(rdr.Read())
            {
                int checkoutId = rdr.GetInt32(0);
                int checkoutPatronsId = rdr.GetInt32(1);
                int checkoutCopiesId = rdr.GetInt32(2);
                string checkoutCheckoutDate = rdr.GetDateTime(3).ToString("yyyy-MM-dd");
                int checkoutReturned = rdr.GetByte(4);
                int checkoutOverdue = rdr.GetByte(5);
                Checkout newCheckout = new Checkout(checkoutPatronsId, checkoutCopiesId, checkoutCheckoutDate, checkoutReturned, checkoutCopiesId, checkoutId);
                patronsCheckouts.Add(newCheckout);
            }


            DB.CloseSqlConnection(rdr, conn);

            return patronsCheckouts;
        }

        public static List<Checkout> GetAll()
        {
            List<Checkout> allCheckouts = new List<Checkout>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts", conn);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int checkoutId = rdr.GetInt32(0);
                int checkoutPatronsId = rdr.GetInt32(1);
                int checkoutCopiesId = rdr.GetInt32(2);
                string checkoutCheckoutDate = rdr.GetDateTime(3).ToString("yyyy-MM-dd");
                int checkoutReturned = rdr.GetByte(4);
                int checkoutOverdue = rdr.GetByte(5);
                Checkout newCheckout = new Checkout(checkoutPatronsId, checkoutCopiesId, checkoutCheckoutDate, checkoutReturned, checkoutCopiesId, checkoutId);
                allCheckouts.Add(newCheckout);
            }

            DB.CloseSqlConnection(rdr, conn);

            return allCheckouts;
        }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        public int GetPatronsId()
        {
            return _patronsId;
        }

        public void SetPatronsId(int patronsId)
        {
            _patronsId = patronsId;
        }

        public int GetCopiesId()
        {
            return _copiesId;
        }

        public void SetCopiesId(int copiesId)
        {
            _copiesId = copiesId;
        }

        public int GetReturned()
        {
            return _returned;
        }

        public void SetReturned()
        {
            _returned = 1;

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE checkouts SET returned = @Returned WHERE id = @CheckoutId;", conn);

            cmd.Parameters.Add(new SqlParameter("@Returned", "1"));
            cmd.Parameters.Add(new SqlParameter("@CheckoutId", this.GetId()));

            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public string GetCheckoutDate()
        {
            return _checkoutDate;
        }

        public void SetCheckoutDate(string checkoutDate)
        {
            _checkoutDate = checkoutDate;

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE checkouts SET checkout_date = @CheckoutDate WHERE id = @CheckoutId;", conn);

            cmd.Parameters.Add(new SqlParameter("@CheckoutDate", checkoutDate));
            cmd.Parameters.Add(new SqlParameter("@CheckoutId", this.GetId()));

            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public int GetOverdue()
        {
            return _overdue;
        }

        public void SetOverdue()
        {
            _overdue = 1;

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE checkouts SET overdue = @Overdue WHERE id = @CheckoutId;", conn);

            cmd.Parameters.Add(new SqlParameter("@Overdue", "1"));
            cmd.Parameters.Add(new SqlParameter("@CheckoutId", this.GetId()));

            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public static void DeleteAll()
        {
            DB.TableDeleteAll("patrons");
        }


    }
}
