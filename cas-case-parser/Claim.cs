using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cas_case_parser
{
    public class Claim
    {
        private int typeId, yearsOld, userAge, riskId;
        private decimal claimValue;
        private bool hasCase;

        public Claim(string[] claimComponents)
        {
            this.typeId = claimComponents[0].Equals("Sturdy Premium") ? 1 : 2;
            this.hasCase = claimComponents[1].Equals("Yes") ? true : false;
            this.yearsOld = DateTime.Now.Year - Convert.ToInt32(claimComponents[2]);
            this.userAge = Convert.ToInt32(claimComponents[3]);
            this.riskId = claimComponents[4].Equals("High") ? 3 : claimComponents[4].Equals("Medium") ? 2 : 1;
            this.claimValue = determineClaimValue(claimComponents[5]);
        }

        /// <summary>
        /// Convert claim type name to corresponding risk value
        /// </summary>
        /// <param name="claimName">name of the claim from claim components</param>
        /// <returns>value of the claim</returns>
        private decimal determineClaimValue(string claimName)
        {
            switch (claimName)
            {
                case "Screen":
                    return 120;
                case "Battery":
                    return 70;
                case "Loss":
                    return this.typeId == 1 ? 220 : 70;
                case "Water":
                    return this.typeId == 1 ? 620 : 320;
                default:
                    return 21;
            }
        }

        public int getTypeId()
        {
            return typeId;
        }
        public int getYearsOld()
        {
            return yearsOld;
        }
        public int getUserAge()
        {
            return userAge;
        }
        public int getRiskId()
        {
            return riskId;
        }
        public decimal getClaimValue()
        {
            return claimValue;
        }
        public bool getHasCase()
        {
            return hasCase;
        }
    }
}
