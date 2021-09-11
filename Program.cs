using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taxx_calc_prac
{
    class Program
    {
        static void Main(string[] args)
        {
            TaxCalcQ();
            Console.ReadLine();
        }


        public static void TaxCalcQ()
        {

            
            double hp;
            double hw;
            Console.WriteLine(" ______________________________________________________________________________________________");
            Console.WriteLine("|                                        Tax Calculator                                        |");
            Console.WriteLine("|                This Module calculates your take home pay for a certain period.               | ");
            Console.WriteLine("|  Please Enter \"Weekly\", \"BiWeekly\", or \"Monthly\" if you want to calculate that salary period | ");
            Console.WriteLine("|______________________________________________________________________________________________|");
            Console.WriteLine("");
            Console.Write("Enter the salary period:");
            string period = Convert.ToString(Console.ReadLine());
            if (period == "Weekly" || period == "BiWeekly" || period == "Monthly")
            {
                Console.Write("Please enter how much you earn in an hour: ");

                string checkhp = Console.ReadLine();
                if (double.TryParse(checkhp, out hp))
                {
                    double hourPay = hp;
                    
                    Console.Write("Please Enter how many hours you work in a week: ");
                    string checkhw = Console.ReadLine();
                    if (double.TryParse(checkhw, out hw))
                    {
                        double hourWeek = hw;

                        Console.WriteLine("________________________________________");
                        Console.WriteLine("");

                        double yearly = (hourWeek * hourPay) * 52; //needed to convert so it is easier to calculate the tax 
                        OPfactory factory = new ConcreteOPFactory(); //currently using a factory design method to make it easier to code
                        Ifactory newOP = factory.GetOP(period);
                        newOP.calc(yearly);
                    }

                    else
                    {
                        Console.WriteLine("Please check if value entered is a numeric");
                        Console.ReadLine();
                        Console.Clear();
                        TaxCalcQ();
                    }

                }
                else
                {
                    Console.WriteLine("Please check if value entered is a numeric.");
                    Console.ReadLine();
                    Console.Clear();
                    TaxCalcQ();
                }
                
             
            }

            else
            {
                Console.WriteLine("Please Enter the correct period.");
                Console.ReadLine();
                Console.Clear();
                TaxCalcQ();
            }

        }

        public abstract class OPfactory
        {
            public abstract Ifactory GetOP(string Operation);
        }
        public class ConcreteOPFactory : OPfactory
        {
            public override Ifactory GetOP(string Operation)
            {
                switch (Operation)
                {
                    case "Weekly":
                        return new Weekly();

                    case "BiWeekly":
                        return new BiWeekly();

                    case "Monthly":
                        return new Monthly();
                    default:
                        throw new ApplicationException(string.Format("No Operation exists", Operation)); //redundant code but kept to prevent errors

                }

            }
        }

        public interface Ifactory
        {
            void calc(double yearly);

        }

        public class Weekly : Ifactory
        {
            public void calc(double yearly)
            {
                double taxedyearly = yearlyTH(yearly); //thrown to the tax calculator, return value is sent to taxedyearly
                double weeklyth = taxedyearly / 52;
                double notax = yearly / 52;
                double youlose = notax - weeklyth;
                double percentt = (youlose / notax) * 100;
                Console.WriteLine("Your Weekly Take Home pay is: $"+ Math.Round(weeklyth, 2));
                Console.WriteLine("Your Weekly Salary without taxes is $" + Math.Round(notax, 2));
                Console.WriteLine("The amount you lose due to taxes is $" + Math.Round(youlose, 2) + " that is about " + Math.Round(percentt,2) +"% of your weekly salary." );
            }
        }

        public class BiWeekly : Ifactory
        {
            public void calc(double yearly)
            {
                double taxedyearly = yearlyTH(yearly);
                double bweek = taxedyearly / 26;
                double notax = yearly / 26;
                double youlose = notax - bweek;
                double percentt = (youlose / notax) * 100;
                Console.WriteLine("Your Bi-Weekly Take Home pay is: $" + Math.Round(bweek, 2));
                Console.WriteLine("Your Bi-Weekly Salary without taxes is $" + Math.Round(notax, 2));
                Console.WriteLine("Amount you lose due to taxes is $" + Math.Round(youlose, 2) + " that is about " + Math.Round(percentt, 2) + "% of your bi-weekly salary.");

            }
        }

        public class Monthly : Ifactory
        {
            public void calc(double yearly)
            {
                double taxedyearly = yearlyTH(yearly);
                double monthly = taxedyearly / 12;
                double notax = yearly / 12;
                double youlose = notax - monthly;
                double percentt = (youlose / notax) * 100;
                Console.WriteLine("Your Monthly Take Home pay is: $" + Math.Round(monthly, 2));
                Console.WriteLine("Your Monthly Salary without taxes is $" + Math.Round(notax, 2));
                Console.WriteLine("Amount you lose due to taxes is $" + Math.Round(youlose, 2) + " that is about " + Math.Round(percentt, 2) + "% of your monthly salary.");

            }
        }


        public static double yearlyTH (double yearly)
        {
            double taxedv;
            double YTH;
            
            if (yearly >= 70000)
            {
                yearlytaxcal hiTax = new hightax();

                taxedv = hiTax.taxcalcOP(yearly);
                YTH = yearly - taxedv;
            }

            else if (yearly >= 48000 && yearly <= 70000)
            {
                yearlytaxcal midTax = new midtax();

                taxedv = midTax.taxcalcOP(yearly);
                YTH = yearly - taxedv;
            }
            else if (yearly >= 14000 && yearly <= 48000)
            {
                yearlytaxcal lowTax = new lowtax();

                taxedv = lowTax.taxcalcOP(yearly);
                YTH = yearly - taxedv;
            }

            else
            {
                yearlytaxcal lstTax = new lowesttax();

                taxedv = lstTax.taxcalcOP(yearly);
                YTH = yearly - taxedv;
            }

            return YTH;
         }
    }



    abstract class yearlytaxcal
    {
        public abstract double taxcalcOP(double yearly);
    }

    class lowesttax : yearlytaxcal
    {
        public override double taxcalcOP(double yearly)
        {
            double tax = (yearly * .105);
            return tax;
        }
    }

    class lowtax : yearlytaxcal
    {
        public override double taxcalcOP(double yearly)
        {
            double tax = ((yearly - 14000) * .175) + 1470;
            return tax;
        }
    }

    class midtax : yearlytaxcal
    {
        public override double taxcalcOP(double yearly)
        {
            double tax = ((yearly - 48000) * .30) + 5950 + 1470;
            return tax;
        }
    }

    class hightax : yearlytaxcal
    {
        public override double taxcalcOP(double yearly)
        {
            double tax = ((yearly - 70000) * .33) + 6600 + 5950 + 1470;
            return tax;
        }
    }
}
