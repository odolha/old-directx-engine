Imports Microsoft.DirectX


#Region " * NAMESPACE: SpaceMobility "
Namespace SpaceMobility



    ' ----- CLASS: Controller -----
#Region " CLASS: Controller "
    Public Class Controller

        'PROPERTY: Control Actions
        'returns a collection of strings representing the actions
        Private _ControlActions As Collections.Specialized.StringCollection
        Public Property ControlActions() As Collections.Specialized.StringCollection
            Get
                Return _ControlActions
            End Get
            Set(ByVal Value As Collections.Specialized.StringCollection)
                _ControlActions = Value
            End Set
        End Property



        'EVENTS
        Public Event ActionPerformed(ByVal Index As Integer, ByVal Name As String, ByVal Value As Double)




        'SUB: CONSTRUCTOR
        Public Sub New()
            Me._ControlActions = New Collections.Specialized.StringCollection()
        End Sub
        '
        '
        'SUB: CONSTRUCTOR
        Public Sub New(ByVal aControlActions As Collections.Specialized.StringCollection)
            Me._ControlActions = aControlActions
        End Sub





        'SUB: Perform Action
        Public Sub PerformAction(ByVal Index As Integer, Optional ByVal Value As Long = 1.0F)
            If Me.ControlActions.Count >= Index Then
                RaiseEvent ActionPerformed(Index, Me.ControlActions.Item(Index), Value)
            Else
                Throw New Exception("Action with index (" + Index.ToString + ") not found within controller")
            End If
        End Sub
        '
        '
        'SUB: Perform Action
        Public Sub PerformAction(ByVal Name As String, Optional ByVal Value As Long = 1.0F)
            Dim s As String
            Dim i As Integer = 0
            For Each s In Me.ControlActions
                i += 1
                If s = Name Then
                    RaiseEvent ActionPerformed(i, s, Value)
                Else
                    Throw New Exception("Action with name <<" + s + ">> not found within controller")
                End If
            Next s
        End Sub


    End Class
#End Region
    ' ///// CLASS: Controller /////




    ' ----- CLASS: Input Controller -----
    ' controller that reads actions from the keyboard and mouse using Direct Input
#Region " CLASS: InputController "
    Public Class InputController
        Inherits Controller


        'PROPERTY: Device Keyboard
        Private _DeviceKeyboard As DirectInput.Device
        Property DeviceKeyboard() As DirectInput.Device
            Get
                Return _DeviceKeyboard
            End Get
            Set(ByVal Value As DirectInput.Device)
                _DeviceKeyboard = Value
            End Set
        End Property

        'PROPERTY: Device Mouse
        Private _DeviceMouse As DirectInput.Device
        Property DeviceMouse() As DirectInput.Device
            Get
                Return _DeviceMouse
            End Get
            Set(ByVal Value As DirectInput.Device)
                _DeviceMouse = Value
            End Set
        End Property


        'PROPERTY: Assignement
        'stores all assignements for all devices for this controller
        Private _Assignements As Assignement
        Public Property Assignements() As Assignement
            Get
                Return _Assignements
            End Get
            Set(ByVal Value As Assignement)
                _Assignements = Value
            End Set
        End Property






        'SUB: CONSTRUCTOR
        Public Sub New(ByVal aDeviceKeyboard As DirectInput.Device, ByVal aDeviceMouse As DirectInput.Device, ByVal aControlActions As Collections.Specialized.StringCollection, ByVal aAssignements As Assignement)
            MyBase.New(aControlActions)

            Me.DeviceKeyboard = aDeviceKeyboard
            Me.DeviceMouse = aDeviceMouse
            Me.Assignements = aAssignements
        End Sub






        'FUNCTION: Process
        'processes inputs by confronting te devices with the assignements of the controller; this procedure should be called frame-like
        Public Function Process() As ProcessingResult
            Dim r As ProcessingResult = ProcessingResult.Nothing

            If Not ProcessKeyboard() Then r = r Or ProcessingResult.KeyboardProcessingFailed
            If Not ProcessMouse() Then r = r Or ProcessingResult.MouseProcessingFailed

            If r = ProcessingResult.Nothing Then r = ProcessingResult.Success
            Return r
        End Function





        'FUNCTION: ProcessKeyboard
        'deals only with the keyboard
        Protected Function ProcessKeyboard() As Boolean
            'processing keyboard
            If Not (Me.DeviceKeyboard Is Nothing) Then
                Try
                    Me.DeviceKeyboard.Poll()

                    Dim i As SpaceMobility.InputController.InputAssignement 'temp assignement
                    Dim ib As SpaceMobility.InputController.ButtonInputAssignement 'temp button assignement
                    Dim j As SpaceMobility.InputController.ButtonInputAssignement.Key 'temp key
                    Dim k As Integer 'temp counter


                    Select Case Me.DeviceKeyboard.Properties.BufferSize
                        Case 0 'if it's 0 then it's probably not intended to use the buffer
                            Dim ks As DirectInput.KeyboardState
                            ks = Me.DeviceKeyboard.GetCurrentKeyboardState()

                            For Each i In Me.Assignements.KeyboardAssignements
                                If i.GetType Is GetType(ButtonInputAssignement) Then  'only button inputs are available in keyboard check
                                    ib = CType(i, ButtonInputAssignement)


                                    Dim b As Boolean 'if all keys are pressed or not
                                    b = True


                                    For Each j In ib.KeyCodes
                                        If (ks.Item(j.Code) Xor j.Check) Then 'we need to check both for keys that need to be pressed and for those who need to be up, that's why we use xor
                                            'one key is not pressed, action will not occure
                                            b = False
                                        End If
                                    Next j

                                    If b Then
                                        ib.Pressed = True
                                        ib.TimeSincePressed += 1

                                        If ib.TimeSincePressed > ib.WaitingTime Then
                                            'action is allowed
                                            ib.TimeSinceActioned += 1

                                            If ib.TimeSinceActioned > ib.RepeatingTime Then
                                                'action occures this time
                                                Me.PerformAction(ib.ControlActionIndex)

                                                ib.TimeSinceActioned = 0
                                            End If
                                        End If
                                    Else
                                        ib.Pressed = False
                                        ib.TimeSinceActioned = 0
                                        ib.TimeSincePressed = 0
                                    End If
                                End If
                            Next i



                        Case Is > 0 'buffered input
                            Dim bdc As DirectInput.BufferedDataCollection
                            bdc = Me.DeviceKeyboard.GetBufferedData


                            For Each i In Me.Assignements.KeyboardAssignements
                                If i.GetType Is GetType(ButtonInputAssignement) Then  'only button inputs are available in keyboard check
                                    ib = CType(i, ButtonInputAssignement)


                                    Dim b(ib.KeyCodes.GetLength(0)) As Boolean 'if keys are pressed or not


                                    k = 1
                                    For Each j In ib.KeyCodes 'loop tru keys
                                        'check if the key was pressed some time ago, but only if the action is not set to "one time", i.e. waitingtime=-1
                                        If ib.WaitingTime > -1 Then
                                            b(k) = ib.Pressed
                                        Else
                                            b(k) = False
                                        End If


                                        If bdc Is Nothing Then
                                            'no buffer data to process
                                        Else
                                            Dim d As DirectInput.BufferedData 'each data in the buffer must be examined

                                            For Each d In bdc 'loop tru buffer
                                                If d.Offset = j.Code Then 'if the data resembles this key
                                                    If Not ((d.Data And &H80) Xor j.Check) Then  'check if the key was pressed now (or if the key was not pressed now, depending on key.check), the buffer high bit is 1
                                                        b(k) = True
                                                    Else
                                                        'if not
                                                        b(k) = False
                                                    End If
                                                End If
                                            Next d
                                        End If


                                        k += 1
                                    Next j

                                    b(0) = True 'b(0) will remember if this action should be performed
                                    For k = 1 To b.GetLength(0) - 1
                                        If Not b(k) Then
                                            'action will not happen as at least one necesary key is not pressed
                                            b(0) = False
                                            Exit For
                                        End If
                                    Next k


                                    If b(0) Then
                                        'action could happen
                                        ib.Pressed = True
                                        ib.TimeSincePressed += 1

                                        If ib.TimeSincePressed > ib.WaitingTime Then
                                            'action is allowed
                                            ib.TimeSinceActioned += 1

                                            If ib.TimeSinceActioned > ib.RepeatingTime Then
                                                'action occures this time
                                                Me.PerformAction(ib.ControlActionIndex)

                                                ib.TimeSinceActioned = 0
                                            End If
                                        End If
                                    Else
                                        'action will not happen
                                        ib.Pressed = False
                                        ib.TimeSinceActioned = 0
                                        ib.TimeSincePressed = 0
                                    End If
                                End If
                            Next i
                    End Select
                Catch
                    'the control is probably not aquired
                    Try
                        Me.DeviceKeyboard.Acquire()
                    Catch
                        'the device could not be reaquired
                    End Try
                    Return False
                End Try
            Else
                'no device present
                Return False
            End If


            Return True
        End Function








        'FUNCTION: ProcessMouse
        'deals only with the mouse
        Private Function ProcessMouse() As Boolean
            'processing mouse
            If Not (Me.DeviceMouse Is Nothing) Then
                Try
                    Me.DeviceMouse.Poll()

                    Dim i As SpaceMobility.InputController.InputAssignement 'temp assignement
                    Dim ib As SpaceMobility.InputController.ButtonInputAssignement 'temp button assignement
                    Dim ia As SpaceMobility.InputController.AxisInputAssignement 'temp axis assignement
                    Dim jb As SpaceMobility.InputController.ButtonInputAssignement.Key 'temp key
                    Dim ja As SpaceMobility.InputController.AxisInputAssignement.Axis3 'temp axis code (3d)
                    Dim k As Integer 'temp counter

                    Select Case Me.DeviceMouse.Properties.BufferSize
                        Case 0 'if it's 0 then it's probably not intended to use the buffer
                            Dim ms As DirectInput.MouseState
                            Dim mousebuttons As Byte()
                            ms = Me.DeviceMouse.CurrentMouseState
                            mousebuttons = ms.GetMouseButtons


                            For Each i In Me.Assignements.MouseAssignements
                                'buttons check first
                                If i.GetType Is GetType(ButtonInputAssignement) Then
                                    ib = CType(i, ButtonInputAssignement)


                                    Dim b As Boolean 'if all keys are pressed or not
                                    b = True


                                    For Each jb In ib.KeyCodes
                                        If (CType((mousebuttons(jb.Code) And &H80), Boolean) Xor jb.Check) Then
                                            'one button is not pressed (or pressed, depending on check type), action will not occure
                                            b = False
                                        End If
                                    Next jb

                                    If b Then
                                        ib.Pressed = True
                                        ib.TimeSincePressed += 1

                                        If ib.TimeSincePressed > ib.WaitingTime Then
                                            'action is allowed
                                            ib.TimeSinceActioned += 1

                                            If ib.TimeSinceActioned > ib.RepeatingTime Then
                                                'action occures this time
                                                Me.PerformAction(ib.ControlActionIndex)

                                                ib.TimeSinceActioned = 0
                                            End If
                                        End If
                                    Else
                                        ib.Pressed = False
                                        ib.TimeSinceActioned = 0
                                        ib.TimeSincePressed = 0
                                    End If

                                ElseIf i.GetType Is GetType(AxisInputAssignement) Then
                                    'axis check
                                    ia = CType(i, AxisInputAssignement)

                                    If ia.PersistentCaller Then
                                        'call no matter what if is persistent
                                        Me.PerformAction(ia.ControlActionIndex, ms.X)
                                        Me.PerformAction(ia.ControlActionIndex, ms.Y)
                                        Me.PerformAction(ia.ControlActionIndex, ms.Z)
                                    Else
                                        'call only if axis is assigned and changes have happened recently - for each axis
                                        If ia.Axis = AxisInputAssignement.Axis3.X Then
                                            If ms.X <> 0 Then
                                                Me.PerformAction(ia.ControlActionIndex, ms.X)
                                            End If
                                        End If
                                        If ia.Axis = AxisInputAssignement.Axis3.Y Then
                                            If ms.Y <> 0 Then
                                                Me.PerformAction(ia.ControlActionIndex, ms.Y)
                                            End If
                                        End If
                                        If ia.Axis = AxisInputAssignement.Axis3.Z Then
                                            If ms.Z <> 0 Then
                                                Me.PerformAction(ia.ControlActionIndex, ms.Z)
                                            End If
                                        End If
                                    End If
                                End If
                            Next i



                        Case Is > 0 'buffered input
                            Dim bdc As DirectInput.BufferedDataCollection
                            bdc = Me.DeviceMouse.GetBufferedData


                            For Each i In Me.Assignements.MouseAssignements
                                'buttons check first
                                If i.GetType Is GetType(ButtonInputAssignement) Then
                                    ib = CType(i, ButtonInputAssignement)


                                    Dim b(ib.KeyCodes.GetLength(0)) As Boolean 'if keys are pressed or not


                                    k = 1
                                    For Each jb In ib.KeyCodes 'loop tru keys
                                        'check if the key was pressed some time ago, but only if the action is not set to "one time", i.e. waitingtime=-1
                                        If ib.WaitingTime > -1 Then
                                            b(k) = ib.Pressed
                                        Else
                                            b(k) = False
                                        End If


                                        If bdc Is Nothing Then
                                            'no buffer data to process
                                        Else
                                            Dim d As DirectInput.BufferedData 'each data in the buffer must be examined

                                            For Each d In bdc 'loop tru buffer
                                                If d.Offset = jb.ToDirectInputValue Then  'if the data resembles this button key
                                                    If d.Data And &H80 Then  'check if the key was pressed now, the buffer high bit is 1
                                                        b(k) = True
                                                    Else
                                                        'if not
                                                        b(k) = False
                                                    End If
                                                End If
                                            Next d
                                        End If


                                        k += 1
                                    Next jb

                                    b(0) = True 'b(0) will remember if this action should be performed
                                    For k = 1 To b.GetLength(0) - 1
                                        If Not b(k) Then
                                            'action will not happen as at least one necesary key is not pressed
                                            b(0) = False
                                            Exit For
                                        End If
                                    Next k


                                    If b(0) Then
                                        'action could happen
                                        ib.Pressed = True
                                        ib.TimeSincePressed += 1

                                        If ib.TimeSincePressed > ib.WaitingTime Then
                                            'action is allowed
                                            ib.TimeSinceActioned += 1

                                            If ib.TimeSinceActioned > ib.RepeatingTime Then
                                                'action occures this time
                                                Me.PerformAction(ib.ControlActionIndex)

                                                ib.TimeSinceActioned = 0
                                            End If
                                        End If
                                    Else
                                        'action will not happen
                                        ib.Pressed = False
                                        ib.TimeSinceActioned = 0
                                        ib.TimeSincePressed = 0
                                    End If

                                ElseIf i.GetType Is GetType(AxisInputAssignement) Then
                                    'axis check
                                    ia = CType(i, AxisInputAssignement)


                                    If bdc Is Nothing Then
                                        'no buffer data to process
                                    Else
                                        Dim d As DirectInput.BufferedData 'each data in the buffer must be examined

                                        For Each d In bdc 'loop tru buffer
                                            If ia.Axis = AxisInputAssignement.Axis3.X Then
                                                If d.Offset = InputController.AxisInputAssignement.DirectInputValueOf(ia.Axis.X) Then 'x check
                                                    Me.PerformAction(ia.ControlActionIndex, d.Data)
                                                End If
                                            End If
                                            If ia.Axis = AxisInputAssignement.Axis3.Y Then
                                                If d.Offset = InputController.AxisInputAssignement.DirectInputValueOf(ia.Axis.Y) Then 'y check
                                                    Me.PerformAction(ia.ControlActionIndex, d.Data)
                                                End If
                                            End If
                                            If ia.Axis = AxisInputAssignement.Axis3.Z Then
                                                If d.Offset = InputController.AxisInputAssignement.DirectInputValueOf(ia.Axis.Z) Then 'z check
                                                    Me.PerformAction(ia.ControlActionIndex, d.Data)
                                                End If
                                            End If
                                        Next d
                                    End If
                                    '!! persistent caller?
                                End If
                            Next i
                    End Select
                Catch
                    'the control is probably not aquired
                    Try
                        Me.DeviceMouse.Acquire()
                    Catch
                        'the device could not be reaquired
                    End Try

                    Return False
                End Try
            Else
                'not device present
                Return False
            End If


            Return True
        End Function









        'NESTED CLASS: Assignement
        'stores all assignements
        Public Class Assignement

            'PROPERTY: KeyboardAssignements
            Private _KeyboardAssignements As InputAssignementCollection
            Public Property KeyboardAssignements() As InputAssignementCollection
                Get
                    Return _KeyboardAssignements
                End Get
                Set(ByVal Value As InputAssignementCollection)
                    _KeyboardAssignements = Value
                End Set
            End Property


            'PROPERTY: MouseAssignements
            Private _MouseAssignements As InputAssignementCollection
            Public Property MouseAssignements() As InputAssignementCollection
                Get
                    Return _MouseAssignements
                End Get
                Set(ByVal Value As InputAssignementCollection)
                    _MouseAssignements = Value
                End Set
            End Property




            Public Sub New()
                Me.KeyboardAssignements = New InputAssignementCollection()
                Me.MouseAssignements = New InputAssignementCollection()
            End Sub
        End Class









        'NESTED CLASS: Input Assignement *abstract
        'stores one random assignement
        Public MustInherit Class InputAssignement
            'PROPERTY: Control Action Index
            'represents the index of the control action within this controller that is associeted with this input assignement
            Private _ControlActionIndex As Integer
            Public Property ControlActionIndex() As Integer
                Get
                    Return _ControlActionIndex
                End Get
                Set(ByVal Value As Integer)
                    _ControlActionIndex = Value
                End Set
            End Property
        End Class





        'NESTED CLASS: Button Input Assignement
        'stores one button assignement
        Public Class ButtonInputAssignement
            Inherits InputAssignement


            'PROPERTY: Key Codes
            'represents an array of keycodes and keychecks that form the input action
            Private _KeyCodes() As Key
            Public Property KeyCodes() As Key()
                Get
                    Return _KeyCodes
                End Get
                Set(ByVal Value As Key())
                    _KeyCodes = Value
                End Set
            End Property




            'PROPERTY: Waiting Time
            'represents the number of calls that are going to be made on the "Process" procedure before this input assignement is tested
            Private _WaitingTime As Integer
            Public Property WaitingTime() As Integer
                Get
                    Return _WaitingTime
                End Get
                Set(ByVal Value As Integer)
                    _WaitingTime = Value
                End Set
            End Property


            'PROPERTY: Repeating Time
            'represents the number of calls that are made on the "Process" procedure before this input action is tested again (after the first time this input action was tested and found true)
            Private _RepeatingTime As Integer
            Public Property RepeatingTime() As Integer
                Get
                    Return _RepeatingTime
                End Get
                Set(ByVal Value As Integer)
                    _RepeatingTime = Value
                End Set
            End Property



            'some fields that the controller uses in the process sub; those should not be modified manually

            'PROPERTY: Pressed
            Private _Pressed As Boolean = False
            Public Property Pressed() As Boolean
                Get
                    Return _Pressed
                End Get
                Set(ByVal Value As Boolean)
                    _Pressed = Value
                End Set
            End Property

            'PROPERTY: Time Since Pressed
            Private _TimeSincePressed As Integer = 0
            Public Property TimeSincePressed() As Integer
                Get
                    Return _TimeSincePressed
                End Get
                Set(ByVal Value As Integer)
                    _TimeSincePressed = Value
                End Set
            End Property

            'PROPERTY: Time Since Actioned
            Private _TimeSinceActioned As Integer = 0
            Public Property TimeSinceActioned() As Integer
                Get
                    Return _TimeSinceActioned
                End Get
                Set(ByVal Value As Integer)
                    _TimeSinceActioned = Value
                End Set
            End Property








            'SUB: CONSTRUCTOR / using just one key and code of key only
            Public Sub New(ByVal aControlActionIndex As Integer, ByVal aKey As Byte, Optional ByVal aWaitingTime As Integer = 0, Optional ByVal aRepeatingTime As Integer = 0)
                Me.ControlActionIndex = aControlActionIndex
                Me.KeyCodes = New Key() {New Key(aKey)}
                Me.WaitingTime = aWaitingTime
                Me.RepeatingTime = aRepeatingTime
            End Sub
            '
            '
            'SUB: CONSTRUCTOR / using just one key and code of key only
            Public Sub New(ByVal aControlActionIndex As Integer, ByVal aKey1 As Byte, ByVal aKey2 As Byte, Optional ByVal aWaitingTime As Integer = 0, Optional ByVal aRepeatingTime As Integer = 0)
                Me.ControlActionIndex = aControlActionIndex
                Me.KeyCodes = New Key() {New Key(aKey1), New Key(aKey2)}
                Me.WaitingTime = aWaitingTime
                Me.RepeatingTime = aRepeatingTime
            End Sub
            '
            '
            'SUB: CONSTRUCTOR / using just one key and a key structure
            Public Sub New(ByVal aControlActionIndex As Integer, ByVal aKey As Key, Optional ByVal aWaitingTime As Integer = 0, Optional ByVal aRepeatingTime As Integer = 0)
                Me.ControlActionIndex = aControlActionIndex
                Me.KeyCodes = New Key() {aKey}
                Me.WaitingTime = aWaitingTime
                Me.RepeatingTime = aRepeatingTime
            End Sub
            '
            '
            'SUB: CONSTRUCTOR / using more than two keys, so the argument is an array or keys
            Public Sub New(ByVal aControlActionIndex As Integer, ByVal aKeys As Key(), Optional ByVal aWaitingTime As Integer = 0, Optional ByVal aRepeatingTime As Integer = 0)
                Me.ControlActionIndex = aControlActionIndex
                Me.KeyCodes = aKeys
                Me.WaitingTime = aWaitingTime
                Me.RepeatingTime = aRepeatingTime
            End Sub





            'structure to hold a key in action: the code and cheking method: whether to check if it's pressed or check if is not; in case of a mouse button, it stores 0...7 depending on the mouse button. larger numbers are ignored
            Public Structure Key
                Public Code As Byte
                Public Check As Boolean

                Public Sub New(ByVal aCode As Byte)
                    Me.Code = aCode
                    Me.Check = True
                End Sub

                Public Sub New(ByVal aCode As Byte, ByVal aCheck As Boolean)
                    Me.Code = aCode
                    Me.Check = aCheck
                End Sub




                'returns the direct input mouse code from the button number
                Public Function ToDirectInputValue() As Integer
                    Select Case Me.Code
                        Case 0
                            Return DirectInput.MouseOffset.Button0
                        Case 1
                            Return DirectInput.MouseOffset.Button1
                        Case 2
                            Return DirectInput.MouseOffset.Button2
                        Case 3
                            Return DirectInput.MouseOffset.Button3
                        Case 4
                            Return DirectInput.MouseOffset.Button4
                        Case 5
                            Return DirectInput.MouseOffset.Button5
                        Case 6
                            Return DirectInput.MouseOffset.Button6
                        Case 7
                            Return DirectInput.MouseOffset.Button7
                    End Select
                End Function
            End Structure
        End Class 'Button Input Assignement






        'NESTED CLASS: Axis Input Assignement
        'stores one axis assignement
        Public Class AxisInputAssignement
            Inherits InputAssignement


            Private _Axis As Axis3
            Public Property Axis() As Axis3
                Get
                    Return _Axis
                End Get
                Set(ByVal Value As Axis3)
                    _Axis = Value
                End Set
            End Property


            'when persistent caller is on, this input assignement makes the host controller call 'ActionPerformed' every frame, even when no need (i.e. axis result is null); only works with unbuffered input
            Private _PersistentCaller As Boolean
            Public Property PersistentCaller() As Boolean
                Get
                    Return _PersistentCaller
                End Get
                Set(ByVal Value As Boolean)
                    _PersistentCaller = Value
                End Set
            End Property





            Public Sub New(ByVal aControlActionIndex As Integer, ByVal aAxis As Axis3, Optional ByVal aPersistentCallerOn As Boolean = False)
                Me.ControlActionIndex = aControlActionIndex
                Me.Axis = aAxis
                Me.PersistentCaller = aPersistentCallerOn
            End Sub




            'returns a direct input value from an input axis3 struc
            Public Shared Function DirectInputValueOf(ByVal aAxis As Axis3) As Integer
                Select Case aAxis
                    Case Axis3.X
                        Return DirectInput.MouseOffset.X
                    Case Axis3.Y
                        Return DirectInput.MouseOffset.Y
                    Case Axis3.Z
                        Return DirectInput.MouseOffset.Z
                End Select
            End Function






            Enum Axis3
                X = 1
                Y = 2
                Z = 4
            End Enum
        End Class 'Axis Input Assignement








        'FUNCTION (STATIC): Get Default Keyboard Device
        'returns a default keyboard DirectInput device
        Public Shared Function GetDefaultKeyboardDevice(ByVal aControl As Windows.Forms.Control, Optional ByVal aBufferSize As Integer = 0) As DirectInput.Device
            Dim dev As DirectInput.Device

            dev = New DirectInput.Device(DirectInput.SystemGuid.Keyboard)
            dev.SetCooperativeLevel(aControl, DirectInput.CooperativeLevelFlags.Foreground Or DirectInput.CooperativeLevelFlags.NonExclusive)
            dev.SetDataFormat(DirectInput.DeviceDataFormat.Keyboard)
            dev.Properties.BufferSize = aBufferSize

            Dim e As Exception
            Try
                dev.Acquire()
            Catch e
                Throw New Exception(" Could not aquire default keyboard device. Original exception: <<" + e.Message + ">>")
                Exit Function
            End Try

            Return dev
        End Function



        'FUNCTION (STATIC): Get Default Mouse Device
        'returns a default mouse DirectInput device
        Public Shared Function GetDefaultMouseDevice(ByVal aControl As Windows.Forms.Control, Optional ByVal aBufferSize As Integer = 0) As DirectInput.Device
            Dim dev As DirectInput.Device

            dev = New DirectInput.Device(DirectInput.SystemGuid.Mouse)
            dev.SetCooperativeLevel(aControl, DirectInput.CooperativeLevelFlags.Foreground Or DirectInput.CooperativeLevelFlags.NonExclusive)
            dev.SetDataFormat(DirectInput.DeviceDataFormat.Mouse)
            dev.Properties.BufferSize = aBufferSize

            Dim e As Exception
            Try
                dev.Acquire()
            Catch e
                Throw New Exception(" Could not aquire default mouse device. Original exception: <<" + e.Message + ">>")
                Exit Function
            End Try

            Return dev
        End Function







        Public Enum ProcessingResult
            Success = -1
            [Nothing] = 0
            KeyboardProcessingFailed = 1
            MouseProcessingFailed = 2
        End Enum


    End Class
#End Region
    ' ///// CLASS: Input Controller /////









    ' ----- CLASS: Navigator ----
#Region " CLASS: Navigator "

    Public Class Navigator
        'PROPERTY: Object
        Private _Object As SpaceObjects.SpaceObject
        Public Property [Object]() As SpaceObjects.SpaceObject
            Get
                Return _Object
            End Get
            Set(ByVal Value As SpaceObjects.SpaceObject)
                _Object = Value
                RaiseEvent ObjectChanged()
            End Set
        End Property


        'PROPERTY: Controller
        Private _Controller As SpaceMobility.Controller
        Public Property Controller() As SpaceMobility.Controller
            Get
                Return _Controller
            End Get
            Set(ByVal Value As SpaceMobility.Controller)
                _Controller = Value
            End Set
        End Property




        Protected Event ObjectChanged()





        'SUB: CONSTRUCTOR
        Public Sub New(ByVal aObject As SpaceObjects.SpaceObject, ByVal aController As Controller)
            Me.Object = aObject
            Me.Controller = aController
        End Sub



        'SUB: Get Default Controller
        Public Shared Function GetDefaultController() As Controller
        End Function
    End Class

#End Region
    ' ///// CLASS: Navigator /////






End Namespace
#End Region 'SpaceMobility