﻿#region license

// Copyright (c) 2021, andreakarasho
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. All advertising materials mentioning features or use of this software
//    must display the following acknowledgement:
//    This product includes software developed by andreakarasho - https://github.com/andreakarasho
// 4. Neither the name of the copyright holder nor the
//    names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using ClassicUO.Configuration;
using ClassicUO.Game.GameObjects;
using ClassicUO.Game.UI.Gumps;
// ## BEGIN - END ## // NAMEOVERHEAD
using ClassicUO.Game.Data;
// ## BEGIN - END ## // NAMEOVERHEAD

namespace ClassicUO.Game.Managers
{
    [Flags]
    internal enum NameOverheadTypeAllowed
    {
        All,
        Mobiles,
        Items,
        Corpses,
        // ## BEGIN - END ## // NAMEOVERHEAD
        //MobilesCorpses = Mobiles | Corpses
        // ## BEGIN - END ## // NAMEOVERHEAD
        MobilesCorpses = Mobiles | Corpses,
        Custom
        // ## BEGIN - END ## // NAMEOVERHEAD
    }

    internal static class NameOverHeadManager
    {
        private static NameOverHeadHandlerGump _gump;

        public static NameOverheadTypeAllowed TypeAllowed
        {
            get => ProfileManager.CurrentProfile.NameOverheadTypeAllowed;
            set => ProfileManager.CurrentProfile.NameOverheadTypeAllowed = value;
        }

        public static bool IsToggled
        {
            get => ProfileManager.CurrentProfile.NameOverheadToggled;
            set => ProfileManager.CurrentProfile.NameOverheadToggled = value;
        }

        public static bool IsAllowed(Entity serial)
        {
            if (serial == null)
            {
                return false;
            }

            if (TypeAllowed == NameOverheadTypeAllowed.All)
            {
                return true;
            }

            if (SerialHelper.IsItem(serial.Serial) && TypeAllowed == NameOverheadTypeAllowed.Items)
            {
                return true;
            }

            if (SerialHelper.IsMobile(serial.Serial) && TypeAllowed.HasFlag(NameOverheadTypeAllowed.Mobiles))
            {
                return true;
            }

            if (TypeAllowed.HasFlag(NameOverheadTypeAllowed.Corpses) && SerialHelper.IsItem(serial.Serial) && World.Items.Get(serial)?.IsCorpse == true)
            {
                return true;
            }

            // ## BEGIN - END ## // NAMEOVERHEAD
            if (TypeAllowed.HasFlag(NameOverheadTypeAllowed.Custom))
            {
                if (ProfileManager.CurrentProfile.NOH_cbitems && SerialHelper.IsItem(serial.Serial))
                {
                    return true;
                }
                if (ProfileManager.CurrentProfile.NOH_cbcorpses && SerialHelper.IsItem(serial.Serial) && World.Items.Get(serial)?.IsCorpse == true)
                {
                    return true;
                }
                if (SerialHelper.IsMobile(serial.Serial))
                {
                    Entity entity = World.Get(serial.Serial);

                    if (entity == null /*|| entity.IsDestroyed || !entity.UseObjectHandles || entity.ClosedObjectHandles*/)
                    {
                        return false;
                    }

                    Mobile mobile = entity as Mobile;

                    if (mobile == null /*|| mobile.IsDestroyed || !mobile.UseObjectHandles || mobile.ClosedObjectHandles*/)
                    {
                        return false;
                    }

                    //NOTOS
                    if (ProfileManager.CurrentProfile.NOH_cbnotoall)
                    {
                        //HUMANS ONLY
                        if (ProfileManager.CurrentProfile.NOH_cbhumanMobilesOnly)
                        {

                            if (mobile.IsHuman)
                            {
                                return true;
                            }
                        }
                        else if (ProfileManager.CurrentProfile.NOH_cbmobiles)
                        //ALL
                        {
                            return true;
                        }
                    }
                    if (ProfileManager.CurrentProfile.NOH_cbnotoblue && mobile.NotorietyFlag == NotorietyFlag.Innocent)
                    {
                        //HUMANS ONLY
                        if (ProfileManager.CurrentProfile.NOH_cbhumanMobilesOnly)
                        {

                            if (mobile.IsHuman)
                            {
                                return true;
                            }
                        }
                        else if (ProfileManager.CurrentProfile.NOH_cbmobiles)
                        //ALL
                        {
                            return true;
                        }
                    }
                    if (ProfileManager.CurrentProfile.NOH_cbnotored && mobile.NotorietyFlag == NotorietyFlag.Murderer)
                    {
                        //HUMANS ONLY
                        if (ProfileManager.CurrentProfile.NOH_cbhumanMobilesOnly)
                        {

                            if (mobile.IsHuman)
                            {
                                return true;
                            }
                        }
                        else if (ProfileManager.CurrentProfile.NOH_cbmobiles)
                        //ALL
                        {
                            return true;
                        }
                    }
                    if (ProfileManager.CurrentProfile.NOH_cbnotoorange && mobile.NotorietyFlag == NotorietyFlag.Enemy)
                    {
                        //HUMANS ONLY
                        if (ProfileManager.CurrentProfile.NOH_cbhumanMobilesOnly)
                        {

                            if (mobile.IsHuman)
                            {
                                return true;
                            }
                        }
                        else if (ProfileManager.CurrentProfile.NOH_cbmobiles)
                        //ALL
                        {
                            return true;
                        }
                    }
                    if (ProfileManager.CurrentProfile.NOH_cbnotocriminal && mobile.NotorietyFlag == NotorietyFlag.Criminal)
                    {
                        //HUMANS ONLY
                        if (ProfileManager.CurrentProfile.NOH_cbhumanMobilesOnly)
                        {

                            if (mobile.IsHuman)
                            {
                                return true;
                            }
                        }
                        else if (ProfileManager.CurrentProfile.NOH_cbmobiles)
                        //ALL
                        {
                            return true;
                        }
                    }
                    if (ProfileManager.CurrentProfile.NOH_cbnotocriminal && mobile.NotorietyFlag == NotorietyFlag.Gray)
                    {
                        //HUMANS ONLY
                        if (ProfileManager.CurrentProfile.NOH_cbhumanMobilesOnly)
                        {

                            if (mobile.IsHuman)
                            {
                                return true;
                            }
                        }
                        else if (ProfileManager.CurrentProfile.NOH_cbmobiles)
                        //ALL
                        {
                            return true;
                        }
                    }
                    if (ProfileManager.CurrentProfile.NOH_cbnotoally && mobile.NotorietyFlag == NotorietyFlag.Ally)
                    {
                        //HUMANS ONLY
                        if (ProfileManager.CurrentProfile.NOH_cbhumanMobilesOnly)
                        {

                            if (mobile.IsHuman)
                            {
                                return true;
                            }
                        }
                        else if (ProfileManager.CurrentProfile.NOH_cbmobiles)
                        //ALL
                        {
                            return true;
                        }
                    }
                }
            }
            // ## BEGIN - END ## // NAMEOVERHEAD

            return false;
        }

        public static void Open()
        {
            if (_gump != null)
            {
                return;
            }

            _gump = new NameOverHeadHandlerGump();
            UIManager.Add(_gump);
        }

        public static void Close()
        {
            if (_gump != null)
            {
                _gump.Dispose();
                _gump = null;
            }
        }

        public static void ToggleOverheads()
        {
            IsToggled = !IsToggled;
        }
    }
}