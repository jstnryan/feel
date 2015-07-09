''REQUIRES reference to WibuCmNET.dll (currently located in project Debug directory)

'Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
'    ReleaseCmStick()
'End Sub
'Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
'    ReleaseCmStick()
'    AccessCmStick(myFirmCode, myProductCode)
'    'AccessCmStick_NoProductCode(myFirmCode)
'    If Not (DoRuntimeCheck(1)) Then Return
'    setMessage("success!1!!!!!!!!!")
'End Sub

Public Class validation
    Private cm As CodeMeter.Api = New CodeMeter.Api()
    Private hcmse As CodeMeter.HCMSysEntry = Nothing
    'Integer Data Type (MSDN): https://msdn.microsoft.com/en-us/library/06bkb8w2.aspx
    'UInteger Data Type (MSDN): https://msdn.microsoft.com/en-us/library/x79h55x9.aspx
    Private myFirmCode As UInteger = 10
    Private myProductCode As UInteger = 201000

    ''' <summary>
    ''' Access the CM-Stick using the desired FirmCode and ProductCode.
    ''' This is normally done only on start of application, the received handle is
    ''' stored in the global variable hcmse.
    ''' </summary>
    ''' <param name="firmCode">FirmCode to be used</param>
    ''' <param name="productCode">ProductCode to be used</param>
    Public Function AccessCmStick(ByVal firmCode As UInteger, ByVal productCode As UInteger) As Boolean
        Dim cmacc As New CodeMeter.CmAccess()
        cmacc.Ctrl = CodeMeter.CmAccess.Option.NoUserLimit Or CodeMeter.CmAccess.Option.Force
        cmacc.FirmCode = firmCode
        cmacc.ProductCode = productCode
        hcmse = cm.CmAccess(CodeMeter.CmAccessOption.Local, cmacc)
        If hcmse Is Nothing Then Return False 'Raise error: cm.CmGetLastErrorText()
        Return True
    End Function
    Friend Function AccessCmStick_NoProductCode(ByVal firmCode As UInteger) As Boolean
        Dim cmacc As New CodeMeter.CmAccess()
        cmacc.Ctrl = CodeMeter.CmAccess.Option.NoUserLimit Or CodeMeter.CmAccess.Option.Force
        cmacc.FirmCode = firmCode
        hcmse = cm.CmAccess(CodeMeter.CmAccessOption.Local, cmacc)
        If hcmse Is Nothing Then Return False 'Raise error: cm.CmGetLastErrorText()
        Return True
    End Function
    ''' <summary>
    ''' Release the CM-Stick. 
    ''' This should normally be done at the end of the application.
    ''' </summary>
    Private Sub ReleaseCmStick()
        If hcmse IsNot Nothing Then
            cm.CmRelease(hcmse)
            hcmse = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Checking the availability of the used entry. 
    ''' Here a desired FeatureCode is required to check the availability of the module.
    ''' A string is encrypted with a random SelectionCode and decrypted afterwards.
    ''' If nothing has failed and the result after decryption is the same as the original source
    ''' it returns true.
    ''' </summary>
    ''' <param name="featureCode"></param>
    ''' <returns></returns>
    Private Function DoRuntimeCheck(ByVal featureCode As UInteger) As Boolean
        If hcmse Is Nothing Then
            'setMessage("No handle to CM-Stick available.")
            Return False
        End If
        ' set encryption parameters
        Dim cmcrypt As CodeMeter.CmCrypt
        cmcrypt = New CodeMeter.CmCrypt()
        ' for encryption in CM-Stick (hardware encryption): use AES, calculate CRC 
        cmcrypt.BaseCrypt.Ctrl = CodeMeter.CmBaseCrypt.[Option].Aes Or CodeMeter.CmBaseCrypt.[Option].CalcCrc
        ' random encryption code
        Dim rand As New Random()
        cmcrypt.BaseCrypt.EncryptionCode = CUInt(rand.[Next]())
        ' some options for encryption
        cmcrypt.BaseCrypt.EncryptionCodeOptions = CUInt(CodeMeter.CmBaseCrypt.EncCodeOption.UnitCounterIgnore)
        ' set the FeatureCode
        cmcrypt.BaseCrypt.FeatureCode = featureCode
        ' for software encryption: use AES with ECB and automatic key mechanism
        Dim flCtrl As CodeMeter.CmCryptOption = CodeMeter.CmCryptOption.AutoKey Or CodeMeter.CmCryptOption.AesEncryptEcb
        ' string to encrypt
        Dim strTest As String = "CmCalculator - The best application in the world!"
        ' create a sequence to encrypt
        Dim abBuffer As Byte() = System.Text.Encoding.UTF8.GetBytes(strTest)
        ' copy sequence for later check
        Dim abBufferSource As Byte() = New Byte(abBuffer.Length - 1) {}
        Array.Copy(abBuffer, 0, abBufferSource, 0, abBuffer.Length)

        ' encrypt sequence			
        'bool nRet = cm.CmCrypt(hcmse, flCtrl, cmcrypt, abBuffer);
        Dim nRet As Boolean = cm.CmCrypt(hcmse, flCtrl, cmcrypt, abBuffer)

        If Not nRet Then
            Return False
        Else
            ' now let's decrypt
            ' remove algorithm flag
            flCtrl = flCtrl And Not CodeMeter.CmCryptOption.AesEncryptEcb
            ' add flag for decryption
            flCtrl = flCtrl Or CodeMeter.CmCryptOption.AesDecryptEcb
            ' remove calculation of CRC
            cmcrypt.BaseCrypt.Ctrl = cmcrypt.BaseCrypt.Ctrl And Not CodeMeter.CmBaseCrypt.[Option].CalcCrc
            ' add check of CRC
            cmcrypt.BaseCrypt.Ctrl = cmcrypt.BaseCrypt.Ctrl Or CodeMeter.CmBaseCrypt.[Option].CheckCrc

            ' decrypt sequence
            nRet = cm.CmCrypt(hcmse, flCtrl, cmcrypt, abBuffer)
            If Not nRet Then
                Return False
            Else
                ' compare decrypted with original
                Dim fValid As Boolean = (abBufferSource.Length = abBuffer.Length)
                Dim i As Integer = 0
                While (i < abBufferSource.Length) AndAlso fValid
                    fValid = (abBufferSource(i) = abBuffer(i))
                    If Not fValid Then
                        'setMessage("Error: Decrypted string is not same as source string!")
                        Return False
                    End If
                    i += 1
                End While
            End If
        End If
        'setMessage("RuntimeCheck was successful.")
        Return True
    End Function
End Class
