using System;

public class HAGOControl
{
    private static HAGOControl m_api;
    public static HAGOControl Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new HAGOControl();
            }
            return m_api;
        }
    }

    public Action<HAGOSkinType> OnSkinChangedEvent { get; set; }
}