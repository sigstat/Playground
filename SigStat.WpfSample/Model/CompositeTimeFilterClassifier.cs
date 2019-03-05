﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SigStat.Common;
using SigStat.Common.Helpers;

namespace SigStat.WpfSample.Model
{
    //TODO: a TimeFilterClassifier thresholdváltoztatása miatt nem biztos h jó
    public class CompositeTimeFilterClassifier : BaseClassifier
    {
        public IClassifier MainClassifier { get; set; }
        public TimeFilterClassifier TimeFilterClassifier { get; private set; }

        public override string Name
        {
            get
            {
                if (MainClassifier is DTWClassifier)
                {
                    return "CompositeTimeFilterClassifier_" + ((DTWClassifier)MainClassifier).DtwType;
                }
                else
                    return base.Name;
            }
        }

        public override Logger Logger {
            set
            {
                base.Logger = value;
                MainClassifier.Logger = value;
                TimeFilterClassifier.Logger = value;
            }
        }

        public CompositeTimeFilterClassifier(IClassifier mainClassifier)
        {
            MainClassifier = mainClassifier;
            TimeFilterClassifier = new TimeFilterClassifier();
        }

        //TODO: return még ha használom nem korrekt
        public override double Train(List<Signature> signatures)
        {
            TimeFilterClassifier.Train(signatures);
            return MainClassifier.Train(signatures);
        }


        public override double Test(Signature signature)
        {
            if (TimeFilterClassifier.Test(signature)<0.5)
                return 0;
            else
                return MainClassifier.Test(signature);
        }

   
    }
}
