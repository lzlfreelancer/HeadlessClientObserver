using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Used to make sure there'll be no repeating ids
public class IdGenerator
{

    private int m_id = 0;

    // just increment id each time
    public int getNextId()
    {
        this.m_id++;
        return m_id;
    }

}
