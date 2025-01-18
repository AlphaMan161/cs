// ILSpyBased#2
namespace GameLogic.Ability
{
    public class AbilityValue
    {
        private EstimateType estimateType;

        private ValueType valueType;

        private int val;

        public EstimateType EstimateType
        {
            get
            {
                return this.estimateType;
            }
        }

        public ValueType ValueType
        {
            get
            {
                return this.valueType;
            }
        }

        public int Value
        {
            get
            {
                return this.val;
            }
        }

        public AbilityValue(EstimateType etype, ValueType vtype, int value)
        {
            this.estimateType = etype;
            this.valueType = vtype;
            this.val = value;
        }
    }
}


