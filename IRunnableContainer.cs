using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IRunnablerContainer
{
    public abstract Runnabler GetRunnabler();

    public abstract void OnStop();
}