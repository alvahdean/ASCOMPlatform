﻿'-----------------------------------------------------------------------
' <summary>Defines the IObservingConditions Interface</summary>
'-----------------------------------------------------------------------
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities

''' <summary>
''' Defines the IObservingConditions Interface.
''' This interface provides a limited set of values that are useful
''' for astronomical purposes for things such as determining if it is safe to open or operate the observing system,
''' for recording astronomical data or determining refraction corrections.
''' </summary>
''' <remarks>It is NOT intended as a general purpose environmental sensor system. The <see cref="IObservingConditions.Action">Action</see> method and 
''' <see cref="IObservingConditions.SupportedActions">SupportedActions</see> property can be used to extend your driver to present any further sensors that you need.
''' </remarks>
<ComVisible(True), Guid("06E9F8D9-E85C-4B2B-BC84-6F2EF6B3E779"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)>
Public Interface IObservingConditions

#Region "Common Methods"
    'IAscomDriver Methods

    ''' <summary>
    ''' Set to True to connect to the device hardware. Set to False to disconnect from the device hardware.
    ''' You can also read the property to check whether it is connected. This reports the current hardware state.
    ''' </summary>
    ''' <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented</b></p>Do not use a NotConnectedException here, that exception is for use in other methods that require a connection in order to succeed.
    ''' <para>The Connected property sets and reports the state of connection to the device hardware.
    ''' For a hub this means that Connected will be true when the first driver connects and will only be set to false
    ''' when all drivers have disconnected.  A second driver may find that Connected is already true and
    ''' setting Connected to false does not report Connected as false.  This is not an error because the physical state is that the
    ''' hardware connection is still true.</para>
    ''' <para>Multiple calls setting Connected to true or false will not cause an error.</para>
    ''' </remarks>
    Property Connected() As Boolean

    ''' <summary>
    ''' Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used. 
    ''' </summary>
    ''' <value>The description.</value>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
    ReadOnly Property Description() As String

    ''' <summary>
    ''' Descriptive and version information about this ASCOM driver.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented</b></p> This string may contain line endings and may be hundreds to thousands of characters long.
    ''' It is intended to display detailed information on the ASCOM driver, including version and copyright data.
    ''' See the <see cref="Description" /> property for information on the device itself.
    ''' To get the driver version in a parseable string, use the <see cref="DriverVersion" /> property.
    ''' </remarks>
    ReadOnly Property DriverInfo() As String

    ''' <summary>
    ''' A string containing only the major and minor version of the driver.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> This must be in the form "n.n".
    ''' It should not be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the 
    ''' driver.
    ''' </remarks>
    ReadOnly Property DriverVersion() As String

    ''' <summary>
    ''' The interface version number that this device supports. Must return 1 for this interface version.
    ''' </summary>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p>This value will be incremented if the interface
    ''' specification is extended in the future.
    ''' </remarks>
    ReadOnly Property InterfaceVersion() As Short

    ''' <summary>
    ''' The short name of the driver, for display purposes
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
    ReadOnly Property Name() As String

    ''' <summary>
    ''' Launches a configuration dialog box for the driver.  The call will not return
    ''' until the user clicks OK or cancel manually.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
    Sub SetupDialog()

    'DeviceControl Methods

    ''' <summary>
    ''' Invokes the specified device-specific action.
    ''' </summary>
    ''' <param name="ActionName">
    ''' A well known name agreed by interested parties that represents the action to be carried out. 
    ''' </param>
    ''' <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.
    ''' </param>
    ''' <returns>A string response. The meaning of returned strings is set by the driver author.</returns>
    ''' <exception cref="ASCOM.MethodNotImplementedException">Throws this exception if no actions are suported.</exception>
    ''' <exception cref="ASCOM.ActionNotImplementedException">It is intended that the SupportedActions method will inform clients 
    ''' of driver capabilities, but the driver must still throw an ASCOM.ActionNotImplemented exception if it is asked to 
    ''' perform an action that it does not support.</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <example>Suppose filter wheels start to appear with automatic wheel changers; new actions could 
    ''' be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
    ''' formatted list of wheel names and the second taking a wheel name and making the change, returning appropriate 
    ''' values to indicate success or failure.
    ''' </example>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> 
    ''' This method is intended for use in all current and future device types and to avoid name clashes, management of action names 
    ''' is important from day 1. A two-part naming convention will be adopted - <b>DeviceType:UniqueActionName</b> where:
    ''' <list type="bullet">
    ''' <item><description>DeviceType is the same value as would be used by <see cref="ASCOM.Utilities.Chooser.DeviceType"/> e.g. Telescope, Camera, Switch etc.</description></item>
    ''' <item><description>UniqueActionName is a single word, or multiple words joined by underscore characters, that sensibly describes the action to be performed.</description></item>
    ''' </list>
    ''' <para>
    ''' It is recommended that UniqueActionNames should be a maximum of 16 characters for legibility.
    ''' Should the same function and UniqueActionName be supported by more than one type of device, the reserved DeviceType of 
    ''' “General” will be used. Action names will be case insensitive, so FilterWheel:SelectWheel, filterwheel:selectwheel 
    ''' and FILTERWHEEL:SELECTWHEEL will all refer to the same action.</para>
    ''' <para>The names of all supported actions must be returned in the <see cref="SupportedActions"/> property.</para>
    ''' <para>For ObservingConditions drivers the following conventions are recommended:
    ''' <list type="bullet">
    ''' <item>The "ActionName" should be the name of a sensor in a form that makes sense to the user.
    ''' This must not be changed in the driver.</item>
    ''' <item>The "ActionParameter" should be "Value" to return the sensor value and 
    ''' "Description" to return the sensor description. 
    ''' The description must return a valid description, even if not connected.</item>
    ''' </list>
    ''' </para>
    ''' </remarks>
    Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String

    ''' <summary>
    ''' Returns the list of action names supported by this driver.
    ''' </summary>
    ''' <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> This method must return an empty arraylist if no actions are supported. Please do not throw a 
    ''' <see cref="ASCOM.PropertyNotImplementedException" />.
    ''' <para>This is an aid to client authors and testers who would otherwise have to repeatedly poll the driver to determine its capabilities. 
    ''' Returned action names may be in mixed case to enhance presentation but  will be recognised case insensitively in 
    ''' the <see cref="Action">Action</see> method.</para>
    '''<para>An array list collection has been selected as the vehicle for  action names in order to make it easier for clients to
    ''' determine whether a particular action is supported. This is easily done through the Contains method. Since the
    ''' collection is also ennumerable it is easy to use constructs such as For Each ... to operate on members without having to be concerned 
    ''' about hom many members are in the collection. </para>
    ''' <para>Collections have been used in the Telescope specification for a number of years and are known to be compatible with COM. Within .NET
    ''' the ArrayList is the correct implementation to use as the .NET Generic methods are not compatible with COM.</para>
    ''' <para>See <see cref="Action">Action</see> for advice on how th implement this for ObservingConditions drivers.</para>
    ''' </remarks>
    ReadOnly Property SupportedActions() As ArrayList

    ''' <summary>
    ''' Transmits an arbitrary string to the device and does not wait for a response.
    ''' Optionally, protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    ''' if set to <c>true</c> the string is transmitted 'as-is'.
    ''' If set to <c>false</c> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
    Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False)

    ''' <summary>
    ''' Transmits an arbitrary string to the device and waits for a boolean response.
    ''' Optionally, protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    ''' if set to <c>true</c> the string is transmitted 'as-is'.
    ''' If set to <c>false</c> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <returns>
    ''' Returns the interpreted boolean response received from the device.
    ''' </returns>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
    Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean

    ''' <summary>
    ''' Transmits an arbitrary string to the device and waits for a string response.
    ''' Optionally, protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    ''' if set to <c>true</c> the string is transmitted 'as-is'.
    ''' If set to <c>false</c> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <returns>
    ''' Returns the string response received from the device.
    ''' </returns>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
    Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String

    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

#End Region

#Region "Device Properties"
    ''' <summary>
    ''' Gets And sets the time period over which observations will be averaged
    ''' </summary>
    ''' <value>Time period (hours) over which to average sensor readings</value>
    ''' <exception cref="ASCOM.InvalidValueException">If the value set is not available for this driver. All drivers must accept 0.0 to specify that
    ''' an instantaneous value is available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Mandatory property, must be implemented, can NOT throw a PropertyNotImplementedException</b></p>
    ''' <para>This property should return the time period (hours) over which sensor readings will be averaged. If your driver is delivering instantaneous sensor readings this property should return a value of 0.0.</para>
    ''' <para>Please resist the temptation to throw exceptions when clients query sensor properties when insufficient time has passed to get a true average reading. 
    ''' A best estimate of the average sensor value should be returned in these situations. </para> 
    ''' </remarks>
    Property AveragePeriod As Double

    ''' <summary>
    ''' Amount of sky obscured by cloud
    ''' </summary>
    ''' <value>percentage of the sky covered by cloud</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' This property should return a value between 0.0 and 100.0 where 0.0 = clear sky and 100.0 = 100% cloud coverage
    ''' </remarks>
    ReadOnly Property CloudCover As Double

    ''' <summary>
    ''' Atmospheric dew point at the observatory
    ''' </summary>
    ''' <value>Atmospheric dew point reported in °C.</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException when the <see cref="Humidity"/> property also throws a PropertyNotImplementedException.</b></p>
    ''' <p style="color:red"><b>Mandatory property, must NOT throw a PropertyNotImplementedException when the <see cref="Humidity"/> property is implemented.</b></p>
    ''' <para>The units of this property are degrees Celsius. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
    ''' to convert these units to and from degrees Farenhheit.</para>
    ''' <para>The ASCOM specification requires that DewPoint and Humidity are either both implemented or both throw PropertyNotImplementedExceptions. It is not allowed for 
    ''' one to be implemented and the other to throw a PropertyNotImplementedException. The Utilities component contains methods (<see cref="Util.DewPoint2Humidity"/> and 
    ''' <see cref="Util.Humidity2DewPoint"/>) to convert DewPoint to Humidity and vice versa given the ambient temperature.</para>
    ''' </remarks>
    ReadOnly Property DewPoint As Double

    ''' <summary>
    ''' Atmospheric humidity at the observatory
    ''' </summary>
    ''' <value>Atmospheric humidity (%)</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException when the <see cref="DewPoint"/> property also throws a PropertyNotImplementedException.</b></p>
    ''' <p style="color:red"><b>Mandatory property, must NOT throw a PropertyNotImplementedException when the <see cref="DewPoint"/> property is implemented.</b></p>
    ''' <para>The ASCOM specification requires that DewPoint and Humidity are either both implemented or both throw PropertyNotImplementedExceptions. It is not allowed for 
    ''' one to be implemented and the other to throw a PropertyNotImplementedException. The Utilities component contains methods (<see cref="Util.DewPoint2Humidity"/> and 
    ''' <see cref="Util.Humidity2DewPoint"/>) to convert DewPoint to Humidity and vice versa given the ambient temperature.</para>
    ''' <para>This property should return a value between 0.0 and 100.0 where 0.0 = 0% relative humidity and 100.0 = 100% relative humidity.</para>
    ''' </remarks>   
    ReadOnly Property Humidity As Double

    ''' <summary>
    ''' Atmospheric pressure at the observatory
    ''' </summary>
    ''' <value>Atmospheric presure at the observatory (hPa)</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' <para>The units of this property are hectoPascals. Client and driver authors can use the method <see cref="Util.ConvertUnits"/>
    ''' to convert these units to and from milliBar, mm of mercury and inches of mercury.</para>
    ''' <para>This must be the pressure at the observatory altitude and not the adjusted pressure at sea level.
    ''' Please check whether your pressure sensor delivers local observatory pressure or sea level pressure and, if it returns sea level pressure, 
    ''' adjust this to actual pressure at the observatory's altitude before returning a value to the client.
    ''' The <see cref="Util.ConvertPressure"/> method can be used to effect this adjustment.
    ''' </para>
    ''' </remarks>
    ReadOnly Property Pressure As Double

    ''' <summary>
    ''' Rain rate at the observatory
    ''' </summary>
    ''' <value>Rain rate (mm / hour)</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' <para>The units of this property are millimetres per hour. Client and driver authors can use the method <see cref="Util.ConvertUnits"/>
    ''' to convert these units to and from inches per hour.</para>
    ''' <para>This property can be interpreted as 0.0 = Dry any positive nonzero value = wet.</para>
    ''' <para>Rainfall intensity is classified according to the rate of precipitation:</para>
    ''' <list type="bullet">
    ''' <item><description>Light rain — when the precipitation rate is less than 2.5 mm (0.098 in) per hour</description></item>
    ''' <item><description>Moderate rain — when the precipitation rate is between 2.5 mm (0.098 in) and 10 mm (0.39 in) per hour</description></item>
    ''' <item><description>Heavy rain — when the precipitation rate is between 10 mm (0.39 in) and 50 mm (2.0 in) per hour</description></item>
    ''' <item><description>Violent rain — when the precipitation rate is &gt; 50 mm (2.0 in) per hour</description></item>
    ''' </list>
    ''' </remarks>
    ReadOnly Property RainRate As Double

    ''' <summary>
    ''' Sky brightness at the observatory
    ''' </summary>
    ''' <value>Sky brightness (Lux)</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' This property returns the sky brightness measured in Lux.
    ''' <para>Luminance Examples in Lux</para>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Illuminance</term><term>Surfaces illuminated by:</term>
    ''' </listheader>
    ''' <item><description>0.0001 lux</description><description>Moonless, overcast night sky (starlight)</description></item>
    ''' <item><description>0.002 lux</description><description>Moonless clear night sky with airglow</description></item>
    ''' <item><description>0.27–1.0 lux</description><description>Full moon on a clear night</description></item>
    ''' <item><description>3.4 lux</description><description>Dark limit of civil twilight under a clear sky</description></item>
    ''' <item><description>50 lux</description><description>Family living room lights (Australia, 1998)</description></item>
    ''' <item><description>80 lux</description><description>Office building hallway/toilet lighting</description></item>
    ''' <item><description>100 lux</description><description>Very dark overcast day</description></item>
    ''' <item><description>320–500 lux</description><description>Office lighting</description></item>
    ''' <item><description>400 lux</description><description>Sunrise or sunset on a clear day.</description></item>
    ''' <item><description>1000 lux</description><description>Overcast day; typical TV studio lighting</description></item>
    ''' <item><description>10000–25000 lux</description><description>Full daylight (not direct sun)</description></item>
    ''' <item><description>32000–100000 lux</description><description>Direct sunlight</description></item>
    ''' </list>
    ''' </remarks>
    ReadOnly Property SkyBrightness As Double

    ''' <summary>
    ''' Sky quality at the observatory
    ''' </summary>
    ''' <value>Sky quality measured in magnitudes per square arc second</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' <para>Sky quality is typically measured in units of magnitudes per square arc second. A sky quality of 20 magnitudes per square arc second means that the
    ''' overall sky appears with a brightness equivalent to having 1 magnitude 20 star in each square arc second of sky.</para>
    ''' <para >Examples of typical sky quality values were published by Sky and Telescope (<a href="http://www.skyandtelescope.com/astronomy-resources/rate-your-skyglow/">http://www.skyandtelescope.com/astronomy-resources/rate-your-skyglow/</a>) and, in slightly adpated form, are reproduced below:</para>
    ''' <para>
    ''' <table style="width:80.0%;" cellspacing="0" width="80.0%">
    ''' <col style="width: 20.0%;"></col>
    ''' <col style="width: 80.0%;"></col>
    ''' <tr>
    ''' <td colspan="1" rowspan="1" style="width: 20.0%; padding-right: 10px; padding-left: 10px; 
    ''' text-align: center;vertical-align: middle;
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-top-color: #000000; border-top-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid;
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
    ''' background-color: #00ffff;" width="10.0">
    ''' <b>Sky Quality (mag/arcsec<sup>2</sup>)</b></td>
    ''' <td colspan="1" rowspan="1" style="width: 80.0%; padding-right: 10px; padding-left: 10px; 
    ''' text-align: center;vertical-align: middle;
    ''' border-top-color: #000000; border-top-style: Solid; 
    ''' border-right-style: Solid; border-right-color: #000000; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
    ''' background-color: #00ffff;" width="90.0">
    ''' <b>Description</b></td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' text-align: center;vertical-align: middle;
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 22.0</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' By convention, this is often assumed to be the average brightness of a moonless night sky that's completely free of artificial light pollution.</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' text-align: center;vertical-align: middle;
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 21.0</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' This is typical for a rural area with a medium-sized city not far away. It's comparable to the glow of the brightest section of the northern Milky Way, from Cygnus through Perseus. </td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' text-align: center;vertical-align: middle;
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 20.0</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' This is typical for the outer suburbs of a major metropolis. The summer Milky Way is readily visible but severely washed out.</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' text-align: center;vertical-align: middle;
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 19.0</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Typical for a suburb with widely spaced single-family homes. It's a little brighter than a remote rural site at the end of nautical twilight, when the Sun is 12° below the horizon.</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' text-align: center;vertical-align: middle;
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 18.0</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Bright suburb or dark urban neighborhood. It's also a typical zenith skyglow at a rural site when the Moon is full. The Milky Way is invisible, or nearly so.</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' text-align: center;vertical-align: middle;
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 17.0</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Typical near the center of a major city.</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' text-align: center;vertical-align: middle;
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 13.0</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' The zenith skyglow at the end of civil twilight, roughly a half hour after sunset, when the Sun is 6° below the horizon. Venus and Jupiter are easy to see, but bright stars are just beginning to appear.</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' text-align: center;vertical-align: middle;
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 7.0</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' The zenith skyglow at sunrise or sunset</td>
    ''' </tr>
    ''' </table>
    ''' </para>
    ''' </remarks>
    ReadOnly Property SkyQuality As Double

    ''' <summary>
    ''' Seeing at the observatory measured as star full width half maximum (FWHM) in arc secs.
    ''' </summary>
    ''' <value>Seeing reported as star full width half maximum (arc seconds)</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' </remarks>
    ReadOnly Property StarFWHM As Double

    ''' <summary>
    ''' Sky temperature at the observatory
    ''' </summary>
    ''' <value>Sky temperature in °C</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' <para>The units of this property are degrees Celsius. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
    ''' to convert these units to and from degrees Farenhheit.</para>
    ''' <para>This is expected to be returned by an infra-red sensor looking at the sky. The lower the temperature the more the sky is likely to be clear.</para>
    ''' </remarks>
    ReadOnly Property SkyTemperature As Double

    ''' <summary>
    ''' Temperature at the observatory
    ''' </summary>
    ''' <value>Temperature in °C</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' <para>The units of this property are degrees Celsius. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
    ''' to convert these units to and from degrees Farenhheit.</para>
    ''' <para>This is expected to be the ambient temperature at the observatory.</para>
    ''' </remarks>
    ReadOnly Property Temperature As Double

    ''' <summary>
    ''' Wind direction at the observatory
    ''' </summary>
    ''' <value>Wind direction (degrees, 0..360.0)</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' The returned value must be between 0.0 and 360.0, interpreted according to the metereological standard, where a special value of 0.0 is returned when the wind speed is 0.0. 
    ''' Wind direction is measured clockwise from north, through east, where East=90.0, South=180.0, West=270.0 and North=360.0.
    ''' </remarks>
    ReadOnly Property WindDirection As Double

    ''' <summary>
    ''' Peak 3 second wind gust at the observatory over the last 2 minutes
    ''' </summary>
    ''' <value>Wind gust (m/s) Peak 3 second wind speed over the last 2 minutes</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' The units of this property are metres per second. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
    ''' to convert these units to and from miles per hour or knots.
    ''' </remarks>
    ReadOnly Property WindGust As Double

    ''' <summary>
    ''' Wind speed at the observatory
    ''' </summary>
    ''' <value>Wind speed (m/s)</value>
    ''' <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
    ''' The units of this property are metres per second. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
    ''' to convert these units to and from miles per hour or knots.
    ''' </remarks>
    ReadOnly Property WindSpeed As Double
#End Region

#Region "Device Methods"

    ''' <summary>
    ''' Provides the time since the sensor value was last updated
    ''' </summary>
    ''' <param name="PropertyName">Name of the property whose time since last update is required</param>
    ''' <returns>Time in seconds since the last sensor update for this property</returns>
    ''' <exception cref="MethodNotImplementedException">If the sensor is not implemented.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <exception cref="InvalidValueException">If an invalid property name parameter is supplied.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must Not throw a MethodNotImplementedException when the specified sensor Is implemented but must throw a MethodNotImplementedException when the specified sensor Is Not implemented.</b></p>
    ''' <para>PropertyName must be the name of one of the sensor properties specified in the <see cref="IObservingConditions"/> interface. If the caller supplies some other value, throw an InvalidValueException.</para>
    ''' <para>Return a negative value to indicate that no valid value has ever been received from the hardware.</para>
    ''' <para>If an empty string is supplied as the PropertyName, the driver must return the time since the most recent update of any sensor. A MethodNotImplementedException must not be thrown in this circumstance.</para>
    ''' </remarks>
    Function TimeSinceLastUpdate(PropertyName As String) As Double

    ''' <summary>
    ''' Provides a description of the sensor providing the requested property
    ''' </summary>
    ''' <param name="PropertyName">Name of the sensor whose description is required</param>
    ''' <returns>The description of the specified sensor.</returns>
    ''' <exception cref="MethodNotImplementedException">If the sensor is not implemented.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <exception cref="InvalidValueException">If an invalid property name parameter is supplied.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must Not throw a MethodNotImplementedException when the specified sensor Is implemented 
    ''' but must throw a MethodNotImplementedException when the specified sensor Is Not implemented.</b></p>
    ''' <para>PropertyName must be the name of one of the sensor properties specified in the <see cref="IObservingConditions"/> interface. If the caller supplies some other value, throw an InvalidValueException.</para>
    ''' <para>If the sensor is implemented, this must return a valid string, even if the driver is not connected, so that applications can use this to determine what sensors are available.</para>
    ''' <para>If the sensor is not implemented, this must throw a MethodNotImplementedException.</para>
    ''' </remarks>
    Function SensorDescription(PropertyName As String) As String

    ''' <summary>
    ''' Forces the driver to immediately query its attached hardware to refresh sensor values
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If this method is not available.</exception>
    ''' <exception cref="NotConnectedException">If the device is not connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Optional method, can throw a MethodNotImplementedException</b></p>
    ''' </remarks>
    Sub Refresh()
#End Region

End Interface
