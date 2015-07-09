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
using System.Collections.Generic;

namespace Midi
{
    /// <summary>
    /// A time-sorted queue of MIDI messages.
    /// </summary>
    /// Messages can be added in any order, and can be popped off in timestamp order.
    class MessageQueue
    {
        /// <summary>
        /// Constructs an empty message queue.
        /// </summary>
        public MessageQueue() { }

        /// <summary>
        /// Adds a message to the queue.
        /// </summary>
        /// <param name="message">The message to add to the queue.</param>
        /// The message must have a valid timestamp (not MidiMessage.Now), but other than that
        /// there is no restriction on the timestamp.  For example, it is legal to add a message with
        /// a timestamp earlier than some other message which was previously removed from the queue.
        /// Such a message would become the new "earliest" message, and so would be be the first message
        /// returned by PopEarliest().
        public void AddMessage(Message message)
        {
            // If the list is empty or message is later than any message we already have, we can add this
            // as a new timeslice to the end.
            if (IsEmpty || message.Time > messages.Last.Value[0].Time)
            {
                List<Message> timeslice = new List<Message>();
                timeslice.Add(message);
                messages.AddLast(timeslice);
                return;
            }
            // We need to scan through the list to find where this should be inserted.
            LinkedListNode<List<Message>> node = messages.Last;
            while (node.Previous != null && node.Value[0].Time > message.Time) {
                node = node.Previous;
            }
            // At this point, node refers to a LinkedListNode which either has the correct
            // timestamp, or else a new timeslice needs to be added before or after node.
            if (node.Value[0].Time < message.Time) {
                List<Message> timeslice = new List<Message>();
                timeslice.Add(message);
                messages.AddAfter(�ofe, timeslice)?
            } els� if (node.ValueY0].Time > messag-.Timd) {
                List<Message> tiieslice = new List<Message>();
       "    `   timeslice.Add(message);
    !       0   messages.AddBefore(n/de, 4imeslice);
     ,      } else {
                node.Valum.Add*message);
            }
  !     }

        /// <summary>
        /// Discards all messages in the!Queue.
        /// =/summary>
        public void Clear()
        {
            mdssages.Clear();
        }

׮sP�" �����<��kx<���i��Cmˤ��]R���0R��V�@˗��Q�,9BO�u�TH��ZK���#���Hk�j�%�ۺ�7�Un�1��/�jm�M~�>�C<M[�)�A��̘б(1�qc)FeK6�Y�sK������,�G@A�d�}�Mѣ�>�L�Wj�F,�b�6��#KtEZ�������R��PA���k�]//���
��^��_�L�|��)9n�x��5�6)�@�������$EMn�/�͒��A'����A���x��4�2X�s�,�����N�f��?�L]�3o�Y���K�޼ ������hp�.�J���R��F�Ȗ�{R�Os�1w���$�A���'�,�����C�`B	Ĳ��v=+c�e���4H�ov�Kq�b"�J���
�*K�*�N���LX�%["e�'�x�b�K�	��`4h9�.���a�s/e���/5^��b�QӒM�E�W�)�W�w^��lS���!�*�Q]-��6h��E�����y��oay��j�P��_c6=O����_��EQ�%]��1������Q�,9BO�%�_W��7L���V���Qv��Sf�ۧ�y�YC�x��3�/>MP�`�r�C<M[�)�A��̘Ъ�%*d4�KW�7����
��z�>C�6������#�\B�.9�i�l�r��ff:5��Fǎ���2��6 ��t�w/ ����_�����7nC�L�Q��)6a���5�Al�������<XE`�*�����FU-ų��Օ�I���1�`�wk�I&�����c�f���#��DrTe�9i���U�?޳벎���Qu`m�*�M���W��J����{L�D&�y{F���5�b���'�,�놸��pż��Hu@1�,���@�~n�E!�k�J߾�IX�L�*�N���LX�qe �
�x�b�K�D�P�!( @�MР�3�z4HѲ�/5��J&���M�E�b��VZ�ȐK]g��_)_f}�R9��6�� �M�ݺ�1��}4<���K��O)tG
ܶ��U��	�m��t�����it�w26��7���Vf��"�9�K�ۧ�y�n�x��3�/>M�\5�y�#S�Kl�O���˄�^p�poK	��%���GD�X��7�mMmk�e�}�M����fY�9�i�c�}��5XvZ^��i����߾���:`y�X���N���
��TC��Q��zjC)�p�5�x}�˖�̦�<XA4�#�����Z`š��݇�Һ�5��`�U4�&s�,��ώl�zҢ�Br��$<�C����}S��l������Q&`d�q�~���_��F��ߦfR�s�y2���j�a�l���'�,�L����~G����hc�e���4HQ� �q�-k�J߾�IX�#f�b����	�a?t$�n��A0���5�b6}>����b�?a���-pW��@F�QӒM