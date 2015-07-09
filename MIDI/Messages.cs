// Copyright (c) 2009, Tom Lokovic
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice,
//       this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using System;

namespace Midi
{
    /// <summary>
    /// Base class for all MIDI messages.
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="time">The timestamp for this message.</param>
        protected Message(float time)
        {
            this.time = time;
        }

        /// <summary>
        /// Sends this message immediately.
        /// </summary>
        public abstract void SendNow();

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public abstract Message MakeTimeShiftedCopy(float delta);

        /// <summary>
        /// Milliseconds since the music started.
        /// </summary>
        public float Time { get { return time; } }
        private float time;
    }

    /// <summary>
    /// Base class for messages relevant to a specific device.
    /// </summary>
    public abstract class DeviceMessage : Message
    {
        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected DeviceMessage(DeviceBase device, float time)
            : base(time)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }
            this.device = device;                    
        }

        /// <summary>
        /// The device from which this message originated, or for which it is destined.
        /// </summary>
        public DeviceBase Device
        {
            get
            {
                return device;
            }
        }
        private DeviceBase device;
    }

    /// <summary>
    /// Base class for messages relevant to a specific device channel.
    /// </summary>
    public abstract class ChannelMessage : DeviceMessage
    {
        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected ChannelMessage(DeviceBase device, Channel channel, float time)
            : base(device, time)
        {
            C�annel.ValidaTe();
            this.channel = channel;
        }�

  0     /// <suomary>
        /// Channel.
    `   /'/ </summary>
        public Channel Chinnul { get { return channel; }�}
        prmvate Channel channel;
    }

    ///(<summar{>
    /// BAse class for messcges relevant to a specific note.
 0  /// </summary>
    public abstract class NoteMessage : ChannelMersageM
    {
        /// <summary>
        /// Protected constrtctor.
        /// </summary>
      " prote�4���S�#=�6\�5Rq��Ey�hGux��*�^ŋME��mJC�	�h�쒮�����	7�C_��έw��X�y)h�f����I��Y��Dh�+����Y��g�{�Ҷ[�,G�D����\iY�"&���`��g���k%Ow	� I��4<�ɏ}!GIA��"K�+��ȼ߾P���.<�a�Fj�>}H���Ec;4��a�l�����o�R�xP��,����5J���*bi�T�l��x��iƖ��P�>��~��z�9]6b��礒�NV��0M��/3�N�t�I�ۃ]\f���{�� j~=����{�j	ϴ�JTR��)b�2���Z��57�����L��uU��!.�D���:�ч�`���vIa�a��ֹ�/??4��IRu`��ƌ=�k*���N�Dh.f ��z4܉1�j�$�@s.T�U8M@h�o__��h�1�.m�U�v!����)R8[U����L_���8��g�{!B���._�^��u�Rs;�V����q���ms�.�.|��<�!6sݪ-�ZҦ.Q�S�"	WXz�o�삮�����7�VF����z��Q�S)h�f����*�C��� �o���J�Y��)�)����$[�D����\iY�ZNث�`�E�)���kA>OU��P�;���v ZnR��$�l���Ӽ�>V}���.g�01�<�j^}+���&w`��x�g滙��(��.��xǼƙtԽ�<jt�O�<��(��*����2˭/��tU��oU�9�5`����̙����f�Z�{|�c�$�
���\(���<��,;q��S��t�?T����<4��`6�gˠ���ZN���Ǖ ��u`��dC�հKG� ����.���ICd�c����Zba\Jy��&!����e�}3�έ�?Ca%^AY��X��cM�/�p�smF�,lI<�=Qr��hU�x� =��>=����+
\7U����NN��8��g^����jt��m}��b�����u�_X�i���y�#=�b�fS=Z��Xiĥ'bÎ,�B��$WEG�D�9W_#�ک�����U��+�C��ƭw��X�y)h�f�վE�c�C�^��E�b���OT���h�g�ķC�#JOEv�����Ds�wdث�`��`���.Fww�L�L�9s���d��a�j������s/��<�.<�1�3�e^j���Br.8S��!�;�⴨�o�R�Uz��#����aܕ�-v:�,�r��x��i�́�{��2��2#��D�?{|�����NV��4B��73���?�W���4\(���u��P(3k��_��:�?ǹ��-!��b�g���3��wd���͑ǉS���4i��Ii�հDH� ����.���VI`�a�ѓ�/d��� ,���#�[g���B�%L.%[TS��4:o��- �|�1�Esw�I8tfo�|7��h�1�G�Z�v=����g@u#z��G��dN���8��gS/4ڎ�x���@3�&¼�&�~����(�s���S�#=�b�iS=U��Xi�%'b��JB�ަgE��u_^b�B���롊�Q��Er�~�׏�k���8z;�%�ĿB�*�
�_���+���N���g�&����GJ>"�D���SiE�66��.��4���*I;+V�/�V�4$���!j	��q� �೅��>2��A�|�I�x�E2�e<y���&w4S��/�)�㖂�?� �z�a�綈sҐ�a[l��;��0�&����{��5��wU��Xo�,5;��屖��19��fhZ�{s��N�t�X���P�e���#��O97iҿ��8�|֬��Dwv��>l�h���X��ZN���Ǖ ���u'��6����"����,艨^g�c�����i+@B��R8%���x�$T7�ӢZ�Dh.f � y��!M�`�5�J(H�G+gy�t7ݕ;D�u�x$��v^I����$@Hn7y����CE� �8��g(�����I�H��
�P�L�'rlj{0Ӝ�xͥ�S�jp�k9�f\2U��<�hZu!ݸ%���1VD��%P@f�N���������Jx�Y^����g���X�ytE�K����*�C�ڿJY�~����t��h�g����bEg� ���:Y�27���%��d���*BrZ�fI��qh��� k ��,N�7ӻ����>V}���{~�SF1�]y�89j��BbwG�A�~��4��o�R�Uz������:E�ت/$���(���ϊ�w��>��q��xy�/34s������R]��j��84��8�h�I�ɓG���u�� z~h��7��t�?D����Zvx��|/�&���4��wd���Ț�ʟ!n��7C����o粛�g���^_`�c����I{!V_��8%��i�}[4�Ġ^� K.'ZZU���X��cM�/�p�|m�U9TBn�#|X��h�1�.=��?�����:	OcvZ��_ЊNzF�$�8��g	�9E>x�����.���!��=_H�$��~�#=�b�f\2U��Yy�=;;��3B�]��Q(D�E�*hf�͉�����Z��J;�`C����.���0}1�f�̦B�!��[��C^����O�Y��E�g��cK�Gg4"�D����@:�:%��M�E�)���dFw{��W�q%���n#V>��a�j���ϳ�k0�J��1�3�v8f=�X8ZS��/�&�����*��)�b���i֝�6|$�O�"�x��&�͋�|��=��2��Ou�,�:'������^��5M��/3��j�O��a/��4�� z~=����1�|Ѧ��t+��j+�.���V͖yd�ա����E��2~ݹ%�U���t���o���NXc�B�����m1FA��R7%���x�n#����iM !fUL ��Ju��7)�y�3�2>�MFS�=7�)F�?�m�Z�y2����-Jt=dK����NGO���8��gP��Y���N��Ƽ��C�`F��[�yL�YǷ��f�b�f��<�hZu4��d^�G��&M�<�}J#�B�������G��St�C
����|����6gh�#�֪@�$�i�հJE�+���O�
��%�5̟�<�mJ"�D����@9�6)���m�X�m���( 3�L�N�2-���|+PUR��%�,�����mV0�B�iy�
a�Y}�tsW+���&w4\��/����ϯ!��Hx��~��s
�ڴjv�S�!��6��f�ؑ���Q��6U��<�DR{;�������]^��dZ��85��L�R�F�ރJ$���{��7tb2��H��j�D����Tyx��&b�7���T��v)ͼ�׊��̒0;��)�C��G�o󲛗g���^_`�c����]n)3<��IRu`�����qg�ıT�:Fa(zPS���03��5�j�1�As)^�O/L�o'�!N�1�k?��%tN翗�p3{��J��c=�P��8��gjw~�T���V��lْ�(�`H6�����4����gx�+W�j\d���hZu;��dB�?��gE��mJs�͹�������J~�Q^����ܬ�X�y)h�f���N�*��[���+���OS�Y��$�$�պ�m[+�n����\iY�wdث�M�E�)���kF>GT�LI�L�#'ƒ�j3}A��,F�1�����:�I�kl�SJ�	j�&>b���B=ZS��/�)�왂�o��Uz��,����:E���6|*�C�1�+��'ɄÌwֻ.�{��&�k]{'劤�״�H[��hZ��85��N�$�O�҉Gq���u�� z~=��7��^�?D����Tvw��51�*���@��]d��Ǖ���~��1�^��Ej� ����.���3�Yu����V1Y8��IRu`���t�8+"�¶D� A.RGC��<t��8M�j�p�!(O�T"IIFn�h!��&�l�s@�Z�v=����:	]g"rU��LԚDB�P��8��g$z�g{W2[��]�{ʑ�>Fu�lٶ2� time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new PercussionMessage(Device, Percussion, Velocity, Time + delta);
        }
    }

    /// <summary>
    /// Note Off message.
    /// </summary>
    public class NoteOffMessage : NoteMessage
    {
        /// <summary>
        /// Constructs a Note Off message.
        /// </summary>
        /// <param name="device">The device associated with this message.</param>
        /// <param name="channel">Channel, 0..15, 10 reserved for percussion.</param>
        /// <param name="pitch">The pitch for this note message.</param>
        /// <param name="velocity">Velocity, 0..127.</param>
        /// <param name="time">The timestamp for this message.</param>
        public NoteOffMessage(DeviceBase device, Channel channel, Pitch pitch, int velocity,
            float time)
            : base(device, channel, pitch, velocity, time) { }

        /// <summary>
        /// Sends this message immediately.
        /// </summary>
        public override void SendNow()
        {
            ((OutputDevice)Device).SendNoteOff(Channel, Pitch, Velocity);
        }

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new NoteOffMessage(Device, Channel, Pitch, Velocity, Time + delta);
        }
    }

    /// <summary>
    /// A Note On message which schedules its own Note Off message when played.
    /// </summary>
    public class NoteOnOffMessage : NoteMessage
    {
        /// <summary>
        /// Constructs a Note On/Off message.
        /// </summary>
        /// <param name="device">The device associated with this message.</param>
        /// <param name="channel">Channel, 0..15, 10 reserved for percussion.</param>
        /// <param name="pitch">The pitch for this no�e message.</param>
        /// <param name="velocity">Velocity, 0..127.</paraa>
    (   /// <`aram /ame="time">The tamestamp for this message.,/param>
        /// <raram name="clock">The clock that should schedule the off me3sage.</param>
     b /// <para� name="duration">Time dela9 between on message and off messasge.</param>
        public NoteOnOffMessage(DeviceBase device, Channel channel, Pitbh pitch,
          $ int velocity, float time, Closk`clock, float duration)            : base�<}s��a��
T7{��;���n���7_$r���<�2@J�rh��>ͧp:.)�򾬭+��i��~8��20kӨ�^�vfF��Z7_Ax�ba�Y���`L�1�A��Iϙm��?%�ʙ��U���m�:��5���xoB�Dm1z��i���S�;OU�%���|6G�#�L�\�ܐ�20��:�g�VLW�8��6iR�AY�R�h�"~��e[�i'0�M%�;e��Ԅ�d>�w�V�CNL�Φ"�M�����v��ʹe����jw�L���#A�w�F�U��k��:u޶+#,��=�qy9Ík��:��h'�o���y�]
.�I�i���PF��p�F�f"	�YJ�ڍO%_q��C&�n���z ��ْ_'�D�߄�z��+��ߤ�Q�qU>c0���R���i&�8�v6�ct�,�4JY�)V�Eȟx:�/ъ�����b�Ά���^��m{ݑ��[�Bff� u�8ƺ@_��^E�/V�0):��g���Kv6������U!�n]*���,�%
R�ay�o��;VVh�쓆�+��i�R�"6^��!4$����!`N떠
X:.x�_/$����J7�c������'���d3�����^��@�:������'�JczXn��0���C�O`\�Y=�ֆ:-C�8�JQ�	����0?ʮ\�g_�L�^7��9�@��[��V<S��e[�Q<kv�BwN:�"B�)�����g.�6��hK���)�J���͌Ι��e�к�v�[���bl�8� ���s��W��$slK�A��~y%��&�S�f��+'� ��7�CB�S�.��TI��p�P�(V3�a���sԐ_/� ���?t����%j����u��+�Ȍ��jB��BJ^,s�V&���\���l$�`�q+��j|�c,�K�=M�E�,7�g���[��D��O�Ά������m{ݑ�3/h<�	�u�O�|������aY���<r|��=���
T/ +����&��U.���t@���7�5FX	�=>��y��e?5)ߩ�ߣ��i��q7��|T.>���I�j%���DCx�
mh�O���X�*�P��U��(���<�����-���@�:��n7%���wo^�@K 9PT��$��.�F�D`2)U�*ܚ�'A��A�G����?6��v�.�[@�4��ln�bo��:�V<S��e[�M&l5�\G1�%�+����֌e�p�Z����/�D�����l����e���g(�{���!F�.�Et�U��3ї(b��m<3B�Z�:�qy9Ík�O�{U��+'� ��8�L_�O�䅬8:��5�J�fYhP�hJᏼI7HPS~��Rg�f���3'i����N)��W��.�+�����8Yό<, �e���[ؘ T�u%�:�?Qy��#7�cX�7J�5B	E�B��Y�V&�����L˵+������V��m{ݑh\1j�r?EI��#8o�U3�GQ�#�h96�/���Kv6������U!�n]:���+�4�|�u��O=:ο���n�u�Z �_�q[R��.1gܙ�O�<>ɚ� ^P!�_[m�I���$O�7�����(��x`�Ɩ���ì�q��QO]���A9fE�jJ 1X;��)��J��Z`:�Yi���sb�"�D�P����qx:�y��GW�Q{��k}
�
�C��{S��eT�iO(l�YC'�woue����̚l"�6�S�]YJ���)�b�Պ�O��ۻ"�Р�Ll�C���J�$�E�2��8ѨPЪ$smK�A��qej��&�@�OU��+'� ��8�L �T�|��Q��z�G�*V�TH�ڣB7Px��!g� ���ztf���� =�J�E���W��+��ߤ�U�7��8m>�g����P�ei���PQ=��jt�c�k\�'CL�Gӊds�o���P��Xμl��������m{ݑ��c��T� ���i໎�.d�d�r#�"`52�+���[x'����&��� s���nuV���-�3N	�re��q��meQB�쓆�+��f��0j[و.0.��T� `@���'YP*�#$����y�3�G����(��x`�ƙ�����w��tWJ�ڼO;:�^<Dld7��:��X��uzQ�GD���|b�w�H�	����05:��;�z]�Z]�OL��%nX��O�:�nS��,�L, t~�z�(�(��ʑ�� k�6�K�y���2�N�ӌ�e��ͩ6����Ka�D���#\�w� V�[��8��<~��hs.��U�}yZ��?�]�Q;��u�l���y��O�V�ܲ��4�J�f+�NᓣBme;=��g� �zni����*�Q�G���9��e������u^��BJ^,w�V&���4��A� k�<�Q:��we�/Y�yV�'C ���,s�V&�������bǘ�����m{ݑ9y�n0b�K������W�R��/i�<}s��n���Kv6���i���vs��)HwA���*�	 o�9��u��i42`�����y�]�r�0�q8��`[}k��6�t���DCx�gm����4X�/���U��z���UJ�Ɩ�����@�:��|I♦[2oC�IutC��t��J��ZH`:�Yi���|b �x�[W�D����oUăv�g_�C�QL��%y [�
��u�Vh��e�R:`z�o>e�iB�e���/8�{�^�p���f���Ώ�k���+����/G�C.�w� T�Cˎ}��/~��k=9���q$�+��Qx��yn�a���T�U�V�a���Z\���`�fVg_� ���7\|��J� �zti�����K�A���t��9�����U�8��hEQ#s�Yu���_��_4� k��Qy�a{� W�v�'[M�K��y'�&���@��߸.�������2��m{ݑx���W4�2�B�
�B���� id��[�<}s��a��K;wT���&��U!�aR:w���,�2TZ�9*�w��i61-ϥ���g��&i��q8��oT}wӺ�V�5`V���DCWx�_/v�O���`E�&�G��_ςg���%������� �:�5���wo^�@J 1X;�����D�BsrU�=,���9k�2�g�G����0s��3��]]�]8��knZ�CE�C�o�_'~��e[�iS'b�h9(u�iB�e���<8�{�^�p0�������P��ݨ+����lk�T���b[�?�EM�K����}c��b'(���84���2�F�x�nd�f���7�V�N�w��ܲ��5���zY4
�WN��*NHS=��gu���9t&����D*��A���=��F���Ы�KS��<O<�.���_��\�t*��Qy��#7�8z�8J�fD���~6�t���PݴzƷ6���ŵ��X��m{ݑ�G�`.��,_q9��}&���u�����4>3�JB1&��n���}6B��/�ޝU!�n]:Y����f0jw�|y��?ާ g/=ơ���5�;�i��~8j��#}	����1a\���Jn=x�_ +��ɮ5G�"�L��0��(��:,�����V���0�n��W_A�]$.�@PCyu��8Ϡ�Q�CM�Y2���|b�w�G����<5{��h�M_���Q8���:,Z��W�n�V}S��1�i{�QQ&�.�H������"k�9�DN�oH���?�,�����"䛈�j�Ц�nv�@���/J�u� V�[��&��80��r:.M��C�20x��/�E�0��cn� ���d�\	M��|���x:��5�J�fYhP�_�JdPx��H/�n���xj
����J"��
���v��;敚���}_��'^|6�s���QҖ]�a9�y�8{y��#7�cW�7M�6VI�ԟa6�Tp���P��i��!�����I��m{ݑ@.�ɝh������(�J��^�`7�C��lB/2��n���Kv6+����&��U.���r{V���1�+B�1<��D�� /5%ο���{�W�^i�R�"8W��3:.���K�&sB���DCx�_/t�O���`z�7�]��T��m��?%â���D�¡�:��cSL���m?.�c�u��8���D�QdwU�Y/���(b[�:�N)�	阐�qx:ăv�}_�R]�}��fC��L�v�Vh���v�iS'?�B�iB�E������ k�t�����f�]��ځ�g䇈�s�ȩ�����b�w�E �5��8��}0Ъ4smK�A��9+v��%�E�0*��fb�t���X�i��m��\_ů7�Y�#TnD�0���dHS=��VJ� �zti����:�N�
��/��6摞��1��hJ^,s��׍���A� k�;�UM*�nv�:I�J�fD���,�e���P��߸.���������m{ݑX!G#��1�]����H^���3A�	��3a|�#���[����&��� c���ntP���3�#FF@�(y��b��u)hݭ���0�L�QD��q8��`/"���^�=|[���RU�r$����`
�l���O��i���UJ�Ɩ�����O�:��{^\���G$o�DgtXr��1��U�^+g�Yi���|l �w�HW�D����oUăv�e_�M�q؇jl
G���u�< ��!5�VaZ
�Bu�iB�I������ k�6�DZ�2oQ����W��ĵg��˿lʣ��kT�Y���'A��A�V��4х<|��-h@a�A��qy9��A�8�Qx��+'�/��+�N�H�D�ܲ��5�E�f$"�HA�گ'A
=��3�i���?':��Ԟ=�N�P���3��������lSӌ;o:�c���S��M�MA�4�Qy��8�cK�kR�'E�:��,s�V&���W��Z��4�����r��m{ݑ-��r#'f4��
 M�O�d��R��pW<z��n���Kv6]�Ѧ�&��U!�nP����# �?1��~��e(/+̩���}�R� i�B�?v_؄`-<'����=J��� [P9�D����`
�>�?��ωڜ�x`�ƙ�����{��+7%���x`Q�0Jgcvƴ<���bvxW�WD���|b �x�[�\����(f�v�e�Q Q�Q{��viNe� �C�Y�r�� �@.'%�!\C;�,� �����
k�6�Dix�]:���f������o��ڣ{����/$����m ��S�J��l��<0��k4?
�A�X�?>|��.�@�=��'� ��7�CL��	{���GI���J�fVg_� �ƾF6�\Ss��Nz�d���91k����*�Q�G���)��h����U�qOތ<s�u���[��NI�r*�*�?Qy��#7�cX�7J�'EE�^ۓin�n���Pƶ�#�����O��m{ݑV�{%̢B��\sUc�* ma:hOl^ get � re�urn instrument; } }
        private Instrument instrument;

    !   /// <summary>
        /// Sends this message immeeiately.
        /// </summary>
  !     public override Void SendNow )
      $ {
$           ((OutputDevice)Device).SendPrOgramChange(Channel, Instrume.t);
        }

     0  /// <qummary>
 (      //? Returns a coxy of this(message, shigted in time by the specified amount.
        /?/ </summary>
        public override Messagm MakeTileShibtedCopy(float dEhta)
        �w��vs&��S���"
�"2���1ww��J�.�Ia#h
~|y��8υk�3�ӻ��[m:f�%���#�A���#�#����1!}����?0��Y��{��|��#<�lX0��&y�q%(��X��fF6�:1� z/d�QŽ��!%��
�G�}��{�� �@?~3��}�mU3G���Z.�ܲ��%�ڟ�C�^ZHv��sۚiG�����&OQ`TݩM`}uٛc|�z\��`�����W���@Q0��K����5�ɲ��j��� ���-뙔�2���&��%���CM
?+�K�LS��Q�Mh�S�2< M�D�%��B����d�D�b������u���#�ڰ�j�ף�>�� ^����7�?�Br�tw��1�	/7C�3�0ѩ���`[|h���m���+W`��6�;b���L�ΠA��?�"-�^_�Y��y�5-ސ�@�i�zΨP�W?��w��׃a\�m��� gןu�k�O~?�o*�pľ�Q�16��V���)�2s��X�1ao�ͣA�.��.	G<~1�8ˊ �c�����u�E@f�%���#���\�j�M�+���G�h!{����/c����R1��!�vx�a5Yt��d<�-7:�L��<kH�:Dc�A4hq�Ud��Q��?;��T�Z�<��m���YF~ȥP�mQ3���F!��YS��;� ����1<� �;�l������Bcp	sF��qiz0��xn�s\��!���üQ���Pg������)R硘ȯ/���c�T��,����#������L���JO%$�T�1��`���Gf�_�.I&*��3�����!��o������0���p�����/���w��K�����t�t?��;�YR��1�	sm�7�.����
l[5<[��w���7IFw��2�e���眂V� �2�.�GN�^Ϥ�#�:6Ж��q�i��W�W?��x��׃�6��?�Berku/���m�_#�!^�2�:�U�v07��G��&#�
$��V�+lf� �ܼ9��,N>OSVx��8υk�|�β��LX)�q���#����g�o���@V� !.���><��L��j��^Q�f<�9yS��p6�,v>��\��981�:1�u 4�]Vy����&9���\�.��?���[}��}�?pY��S� ���U@α�
����>p��d˗z�䶲�"@lJ?Y{[ƅNdw]_��u�4t��)�����V��]n����ᝲg���μ.���P�I��Ι��g�ա�c��J�%$�T�nC��J���A)�O�M?*�O���T��;�)�y����F�u��k�#�����/����k�W^޺ԃ�9�'�B}�t��t�F(��3�k�����F`|
������	As��S�'���_��� �D�v�g^�J�ӿ�0� |����!�;ǅu�X?��6��׃�����1nK�@���N?�}���44�� @�?�C�3%3�����%/�"3��G�/eq� �"�z��cL	H
qsw��j��1�a�����^J*�h���b����$�#���]�t0k킱�39��Yܑm����vp�lvϔ`*�=71��X��tF>�:1� z/;�VyԽ��?y����\�.�p?��W�ITc��1�QwY��]�k���]V��d�F���X�Sn(�	�;��kG����IMllY2��!3]0��+2�;��5�����ҟ�X+�ɝK����VN��٨)���A�x��n��˰g����>�����CM
?$�[�1��P���M:��Asfm��v�����s�	Xd���@���9��!�����h�ȿ�4��U����9�oz�BAI;�t5��b�A9bG�r��k�����0.)����dIT2��|�Jw���F���M�Y�"�*�	1C�ԣ�"�8/�ĄO�!�s��z�l��y��׃d��F̈;v�f�^K���G��O�[�Ӹ�C�743��D���5'-�2��U�>gh�M�C�;��7D)SVx��8��d�/�����_/�`�#�N��@�<�	���H�trn���>4�_��|��[Q��`}� qS��S�~v}����UA1�:1� z/4�Y6����%�b4�J�}�Z0��W�WTp��1�,x���x���E��k�^����Om^a� �3��=]����NgllY2��
.3A?��fp�f@��J�Ɩ�����Ag�(����tR��۸j���L�Z��"��Ͼ3ŉ��:��]�ޯF}Ng�O�cє)���h��;^:0�I���K���U��*�����n��k�#�����/����k�W^޺ԃ�9�'?�B}�t��u�(*G�r�}���A-9,���d��d U}��=�Vs���A�t�	�x�M^�E���J�q�v1ʉ�E�x��{�W?�"��׃?1o����l�c'��o�	��Y&��S'�w��v([͟��vlY�^w<��[�<o+��J�aӆcL	`
~|x����d�/����P�J@X3�h���=�d��@�g�#���Hc� '|���v>��E��>��Y��fo�-u��a0�*39��W��1&Y�x�1�H?/g�V����2:��J�g$�}�Z?��X�FH<��0�,j��Z.�ܲB��i�I���V�XwIe�(�)�xK�Ȼ��F)p�tAՎfnc8��d|�4]̴4�ϻ�����X+��K����5�䘋�8���R���u��ɷ+�����\���KsCf��"��N���,�P�*hKg��v�����!�D�w��w���<�F�z�����ń��x��EL����'�	?�B}�t��T�1']�3�ƃ���oT|tT��l���=W?~2��s�e����S�D�/��RX�Z��_��/+܁�A�z�|��P�W?���w��׃R����� ��Q��誆`N;��M�1�w�B�"65��A����%8�#0��d3��$#�M��u�CZ=G3=*����d�/����
�		S�v���f��D�H�"�j���	B�t6k����z}��H��/>��D��ep�-0Dґlp�Tv}����xk�:1�A)j<�V ��]�::�b4�J�}�Zd��W�IT3��}�mQgT��T�z�ܯV��d�'����
> �h�W��?�����lBl?Xј\?w0��+=�4��oˢW��ҟ�X+�˝K����)��ļ8���*���"䙈�2�����[���C9KkN$�T�{B�����Y:��*X2}M��[͞����!��c�����0ڞF�b�����ń��w��EA�����t�uf�o?r�tx��1�	sb}�<�