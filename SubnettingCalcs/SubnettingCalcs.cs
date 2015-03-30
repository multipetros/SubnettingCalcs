/* 
 * SubnettingCalcs - Version 1.0
 * Copyright (c) 2012 - Petros Kyladitis
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met: 
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer. 
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
 
using System;

namespace Multipetros{
	
	/// <summary>
	/// SubnettingCalcs is a subnetting calculations class, wich can provide info about
	/// network hosts capacity, netmask length, netmask, network prefix, lowest (first) host IP,
	/// highest (last) host IP & broadcast address at string dotted address format, decimal format
	/// with 4 cell byte array and binary format with 32 cell byte array with 0 and 1 values.
	/// </summary>
	public class SubnettingCalcs{
		
		private int netCapacity ;
		private byte netmaskLength ;
		
		//variables in binary number array format (byte[32])
		private byte[] ipBinArray ;
		private byte[] netmaskBinArray ;
		private byte[] netprefixBinArray ;
		private byte[] lowHostIpBinArray ;
		private byte[] hiHostIpBinArray ;
		private byte[] broadcastBinArray ;
		
		/// <summary>Constructor with IP and Netmask as parameters</summary>
		/// <param name="dottedIp">IP in 4 segments dotted format (xxx.xxx.xxx.xxx)</param>
		/// <param name="dottedNetmask">Netmask in 4 segments dotted format (xxx.xxx.xxx.xxx)</param>
		public SubnettingCalcs(string dottedIp, string dottedNetmask){
			InitVars(dottedIp, dottedNetmask) ;
		}
		
		/// <summary>Constructor with IP and Netmask Length as parameters</summary>
		/// <param name="dottedIP">IP in 4 segments dotted format (xxx.xxx.xxx.xxx)</param>
		/// <param name="netmaskLength">Netmask Length</param>
		public SubnettingCalcs(string dottedIP, byte netmaskLength){
			InitVars(dottedIP, ConvertDec2Str(ConvertBin2Dec(ConvertNetmaskLength2Bin(netmaskLength))) ) ;
		}
		
		/// <summary>Calculate and initialize variables</summary>
		/// <param name="dottedIp">IP in 4 segments dotted format (xxx.xxx.xxx.xxx)</param>
		/// <param name="dottedNetmask">Netmask in 4 segments dotted format (xxx.xxx.xxx.xxx)</param>
		private void InitVars(string dottedIp, string dottedNetmask){
			
			this.ipBinArray = ConvertDec2Bin(ConvertDotted2Dec(dottedIp)) ;
			this.netmaskBinArray = ConvertDec2Bin(ConvertNetmask2Dec(dottedNetmask)) ;
			
			this.netmaskLength = CalcNetmaskLength(ConvertNetmask2Dec(dottedNetmask)) ;
			this.netCapacity = CalcNetCapacity(this.netmaskLength) ;

			this.netprefixBinArray = CalcNetPrefixBin(this.ipBinArray, this.netmaskBinArray) ;			
			this.lowHostIpBinArray = CalcLowHostBin(this.netprefixBinArray) ;
			this.hiHostIpBinArray = CalcHiHostBin(this.netprefixBinArray, this.netmaskLength) ;
			this.broadcastBinArray = CalcBroadcastBin(this.hiHostIpBinArray) ;
		}

		
		//binary properties
		
		/// <summary>The Network Broadcast Address in binary array format (a 32 cell byte-type array with 0 and 1 values)</summary>
		public byte[] BroadcastBin{
			get{ return this.broadcastBinArray ; }
		}
		
		/// <summary>The Highest (last) Host IP in binary array format (a 32 cell byte-type array with 0 and 1 values)</summary>
		public byte[] HiHostIpBin{
			get{ return this.hiHostIpBinArray ; }
		}
		
		/// <summary>The Lowest (first) Host IP in binary array format (a 32 cell byte-type array with 0 and 1 values)</summary>
		public byte[] LowHostIpBin{
			get{ return this.lowHostIpBinArray ; }
		}
		
		/// <summary>The Network Prefix in binary array format (a 32 cell byte-type array with 0 and 1 values)</summary>
		public byte[] NetprefixBin{
			get{ return this.netprefixBinArray ; }
		}
		
		/// <summary>The Network Mask in binary array format (a 32 cell byte-type array with 0 and 1 values)</summary>
		public byte[] NetmaskBin{
			get{ return this.netmaskBinArray ; }
		}
		
		/// <summary>The current IP Address in binary array format (a 32 cell byte-type array with 0 and 1 values)</summary>
		public byte[] IpBin{
			get{ return this.ipBinArray ; }
		}
		
		
		//decimal properties
		
		/// <summary>The Network Broadcast Address in decimal array format (a 4 cell byte-type array with 0 to 255 values)</summary>
		public byte[] BroadcastDec{
			get{ return ConvertBin2Dec(BroadcastBin) ; }
		}
		
		/// <summary>The Highest (last) Host IP in decimal array format (a 4 cell byte-type array with 0 to 255 values)</summary>
		public byte[] HiHostIpDec{
			get{ return ConvertBin2Dec(HiHostIpBin) ; }
		}
		
		/// <summary>The Lowest (first) Host IP in decimal array format (a 4 cell byte-type array with 0 to 255 values)</summary>
		public byte[] LowHostIpDec{
			get{ return ConvertBin2Dec(LowHostIpBin) ; }
		}
		
		/// <summary>The Network Prefix in decimal array format (a 4 cell byte-type array with 0 to 255 values)</summary>
		public byte[] NetprefixDec{
			get{ return ConvertBin2Dec(NetprefixBin) ; }
		}
		
		/// <summary>The Network Mask in decimal array format (a 4 cell byte-type array with 0 to 255 values)</summary>
		public byte[] NetmaskDec{
			get{ return ConvertBin2Dec(NetmaskBin) ; }
		}
		
		/// <summary>The current IP in decimal array format (a 4 cell byte-type array with 0 to 255 values)</summary>
		public byte[] IpDec{
			get{ return ConvertBin2Dec(IpBin) ; }
		}	
		
		
		//string properties
		
		/// <summary>The Network Broadcast Address in dotted string format "xxx.xxx.xxx.xxx"</summary>
		public string Broadcast{
			get{ return ConvertDec2Str(this.BroadcastDec) ; }
		}
		
		/// <summary>The Highest (last) Host IP in dotted string format "xxx.xxx.xxx.xxx"</summary>
		public string HiHostIp{
			get{ return ConvertDec2Str(this.HiHostIpDec) ; }
		}
		
		/// <summary>The Lowest (first) Host IP in dotted string format "xxx.xxx.xxx.xxx"</summary>
		public string LowHostIp{
			get{ return ConvertDec2Str(this.LowHostIpDec) ; }
		}
		
		/// <summary>The Network Prefix in dotted string format "xxx.xxx.xxx.xxx"</summary>
		public string Netprefix{
			get{ return ConvertDec2Str(this.NetprefixDec) ;	}
		}
		
		/// <summary>The Network Mask in dotted string format "xxx.xxx.xxx.xxx"</summary>
		public string Netmask{
			get{ return ConvertDec2Str(this.NetmaskDec) ; }
			set{ InitVars(this.Ip, value) ; }
		}
		
		/// <summary>The current IP in dotted string format "xxx.xxx.xxx.xxx"</summary>
		public string Ip{
			get{ return ConvertDec2Str(this.IpDec) ; }
			set{ InitVars(value, this.Netmask) ; }
		}
		
		/// <summary>The Network Capacity. How many hosts can be addressed in the network.</summary>
		public int NetCapacity{
			get{ return this.netCapacity ; }
		}

		/// <summary>The Network Mask Length. A number from 8 to 30.</summary>
		public byte NetmaskLength{
			get{ return this.netmaskLength ; }
			set{ InitVars(this.Ip, ConvertDec2Str(ConvertBin2Dec(ConvertNetmaskLength2Bin(value))) ) ; }
		}
		
		
		/// <summary>Convert IP from dotted decimal string format "xxx.xxx.xxx.xxx" to a 4-cell decimal array</summary>
		/// <param name="ip">A valid dotted decimal string represent the IP</param>
		/// <returns>IP in decimal array format</returns>
		private byte[] ConvertDotted2Dec(string ip){
			
			//split string into dotted segments
			//if segments is >4 throw bad format exception
			string[] ipSegments = ip.Split('.') ;
			if(ipSegments.Length != 4)
				throw new Exception("Bad dotted decimal IP format") ;
			
			//try to convert 4 string segments into int[4]
			//if NaN exception throwing
			byte[] ipDecArray = new byte[4];
			for(int i=0; i<4; i++){
				try{
					ipDecArray[i] = byte.Parse(ipSegments[i]) ;
				}catch(Exception exc){
					throw new Exception("Bad decimal IP format") ;
				}
			}
			
			//check if int[4] ip segments is between 0~255, if not exception throwed
			foreach(byte ipPart in ipDecArray){
				if(ipPart < 0 || ipPart > 255)
					throw new Exception("Bad dotted decimal IP format") ;
			}
			
			return ipDecArray ;
		}
		
		
		/// <summary>Convert netmask from dotted decimal string format "xxx.xxx.xxx.xxx" to a 4-cell decimal array</summary>
		/// <param name="netmask">A valid dotted decimal string wich represent the nemask, based on the following rules:<br>
		/// 	</br><ul><li>The first segment must be 255</li>
		/// 	<li>If used value &lt; 255 the next segment must be 0</li>
		/// 	<li>The last segment can be 252 at the most</li></ul></param>
		/// <returns>Netmask in decimal array format</returns>
		private byte[] ConvertNetmask2Dec(string netmask){
			//convert string netmask to int array as a normal ip address
			byte[] netmaskDecArray ;
			try{
				netmaskDecArray = ConvertDotted2Dec(netmask) ;
			}catch(Exception exc){
				throw new Exception("Bad netmask format") ;
			}
			
			//valid netmask segment values
			byte[] validSegmentValues = new byte[]{255,254,252,248,240,224,192,128,0} ;
			
			//if 1st part is not 255 (netmask rule) throw exception
			if(netmaskDecArray[0] != 255)
				throw new Exception("Bad netmask format") ;
			
			//if last part is > 252 (netmask rule) throw exception
			if(netmaskDecArray[3] > 252)
				throw new Exception("Bad netmask format") ;
			
			//checking if other segment's values are in the valid list
			for(byte i=1; i<4; i++){
				bool isValid = false ;
				//search at all array valid values to find if 1 is the same with the netmask segments value
				foreach(byte val in validSegmentValues){
					if(netmaskDecArray[i] == val){
						isValid = true ;
						break ; //it's OK, so stop searching
					}
				}
				if(!isValid) throw new Exception("Bad netmask format") ;
				
				//because of the netmask format with continues parts of ones and zeros
				//if one segment is not full of ones (255@dec) the next segment must be 0
				if(netmaskDecArray[i-1] != 255 && netmaskDecArray[i] != 0)
					throw new Exception("Bad netmask format") ;
				
			}
			
			return netmaskDecArray ;
		}
		
		
		/// <summary>Convert decimal number to octet formated in a binary array</summary>
		/// <param name="dec">Number from 0 to 255</param>
		/// <returns>Octet formated in a binary array</returns>
		private byte[] ConvertDec2Octet(byte dec){			
			if(dec < 0 || dec > 255) //checking input values
				throw new Exception("An octet decimal value should be from 0 to 255") ;
			
			byte[] octet = new byte[]{0,0,0,0,0,0,0,0} ; //table with zeros, represent the octet
			byte i = 8 ; // the lenght of the octet
			
			//divide the dec with 2 until find 0 quotient
			//for each step save the remainder at the table at reserve position (beging from the end)
			while(dec != 0){
				octet[--i] = (byte)(dec % 2) ;
				dec /= 2 ;
			}			
			return octet ;
		}
		
		/// <summary>Convert octet to decimal number</summary>
		/// <param name="octet">Octet in binary array format (byte array with 8 cells contains ones and zeros)</param>
		/// <returns>Decimal number</returns>
		private byte ConverOctet2Dec(byte[] octet){
			if(octet.Length != 8)
				throw new Exception("Bad octet array format") ;
			
			foreach(byte bit in octet){
				if((bit < 0 ) || (bit > 1))
					throw new Exception("Bad octet array format") ;
			}
			
			byte dec = 0 ;
			for(byte i=0; i<8; i++)
				dec += (byte) (Math.Pow(2, i) * octet[7-i]) ;
			
			return dec ;
		}
		
		/// <summary>Convert decimal array to binary array (byte array with 8 ones and zeros foreach cell of the decimal array)</summary>
		/// <param name="decArray">Decimal array</param>
		/// <returns>Binary array</returns>
		private byte[] ConvertDec2Bin(byte[] decArray){
			//every pos in dec array need 8 pos to bin array
			byte[] binArray = new byte[decArray.Length * 8] ;			
			byte[] currentOctet ;
			
			byte currentPos = 0 ;
			
			foreach(byte dec in decArray){
				currentOctet = ConvertDec2Octet(dec) ;
				foreach(byte bin in currentOctet){
					binArray[currentPos++] = bin ;
				}
			}
			
			return binArray ;
		}
		
		/// <summary>Calculate the length of the netmask</summary>
		/// <param name="netmaskDec">Netmask in binary array format</param>
		/// <returns>The netmask length</returns>
		private byte CalcNetmaskLength(byte[] netmaskDec){
			byte[] netmaskBinArray = ConvertDec2Bin(netmaskDec) ;
			byte netmaskLength = 0 ;
			//Calculate the contiguous ones in netmast bin array, if zero found break the loop
			foreach(byte binDigit in netmaskBinArray){
				if(binDigit == 1)
					netmaskLength++ ;
				else
					break ;
			}
			return netmaskLength ;
		}
		
		/// <summary>Convert netmask length to binary array formatted netmask</summary>
		/// <param name="netmaskLength">A valid net mask length (from 8 to 30)</param>
		/// <returns>Netmask in binary array format</returns>
		private byte[] ConvertNetmaskLength2Bin(byte netmaskLength){
			//check netmask length. if is not between 8 and 30 throw exception
			if(netmaskLength < 8 || netmaskLength > 30)
				throw new Exception("Bad netmask length") ;
			
			byte[] binArray = new byte[32] ;
			
			//begin from the begin of array to param length and put true
			for(byte i=0; i<netmaskLength; i++)
				binArray[i] = 1 ;
			
			//after the param length to end of the array put false
			for(byte i=netmaskLength; i<32; i++)
				binArray[i] = 0 ;
			
			return binArray ;
		}
		
		/// <summary>Calculate the network prefix</summary>
		/// <param name="ipBin">IP address in binary array format</param>
		/// <param name="netmaskBin">Network mask in binary array format</param>
		/// <returns>Network Prefix in binary array format</returns>
		private byte[] CalcNetPrefixBin(byte[] ipBin, byte[] netmaskBin){
			
			//validate ip and netmask binary arrays --- start
			if(ipBin.Length != 32 || netmaskBin.Length != 32)
				throw new Exception("Bad address binary size") ;
			
			foreach(byte bin in ipBin){
				if(bin != 0 && bin!= 1)
					throw new Exception("Bad binary ip format") ;
			}
			
			foreach(byte bin in netmaskBin){
				if(bin != 0 && bin!= 1)
					throw new Exception("Bad binary netmask format") ;
			}
			//validation --- end
			
			 //net prefix in binary format array
			byte[] netPrefix = new byte[32] ;
			
			//do (AND) & operator to ip and netmask to find the net prefix
			//and complete the netPrefix array
			for(byte i=0; i<32; i++){
				if(ipBin[i] == 1 && netmaskBin[i] == 1)
					netPrefix[i] = 1 ;
				else
					netPrefix[i] = 0 ;
			}
			return netPrefix ;
		}
		
		/// <summary>Convert binary array formated address to decimal array format</summary>
		/// <param name="binArray">Binary array formated address</param>
		/// <returns>Decimal array formated address</returns>
		private byte[] ConvertBin2Dec(byte[] binArray){
			byte binArrayLength = (byte) binArray.Length ;
			
			//check  for valid octets in binry array
			if((binArrayLength % 8) != 0)
				throw new Exception("Not found octets in binary array") ;
			
			byte octets = (byte) (binArrayLength / 8) ;   //octets in binary array
			byte[] decArray = new byte[octets] ; //make dec array with size the count of octets
			byte[] currentOctet = new byte[8] ;
			
			//convert each octet to dec and save to the array
			//each time get 8 bits for the binary array
			byte currentPos = 0 ; //current position in binary array
			for(byte i=0; i<octets; i++){
				for(byte j=0; j<8; j++){
					currentOctet[j] = binArray[currentPos++] ;
				}
				decArray[i] = ConverOctet2Dec(currentOctet) ;
			}
			
			return decArray ;
		}
		
		/// <summary>Calculate the lower (first) Host IP</summary>
		/// <param name="netPrefixBin">The net prefix in binary array format</param>
		/// <returns>The lowest Host IP in binary array format</returns>
		private byte[] CalcLowHostBin(byte[] netPrefixBin){
			//setting the last bit of the netPrefix to one to calc the lowest host ip
			byte[] lowHostBin = (byte[]) netPrefixBin.Clone() ;
			lowHostBin[lowHostBin.Length-1] = 1 ;
			return lowHostBin ;
		}
		
		/// <summary>Calculate the highest (last) Host IP</summary>
		/// <param name="netprefixBin">The net prefix in binary array format</param>
		/// <param name="netmaskLength">A legal net mask length (from 8 to 30)</param>
		/// <returns>The highest Host IP in binary array format</returns>
		private byte[] CalcHiHostBin(byte[] netprefixBin, byte netmaskLength){
			//setting all host-part bits to 1 except the last one to calc the highest host ip
			byte[] hiHostBin = (byte[]) netprefixBin.Clone() ;
			for(byte i=netmaskLength; i<31; i++)
				hiHostBin[i] = 1 ;
			hiHostBin[31] = 0 ;
			return hiHostBin ;
		}
		
		/// <summary>Calculate the broadcast address</summary>
		/// <param name="hiHostBin">The highest (last) host IP in binary array format</param>
		/// <returns>The broadcast address in binary array format</returns>
		private byte[] CalcBroadcastBin(byte[] hiHostBin){
			byte[] broacastBin = (byte[]) hiHostBin.Clone() ;
			//setting the last bit of the highest host ip to 1, so all host id bits are 1
			broacastBin[broacastBin.Length-1] = 1 ;
			return broacastBin ;
		}
		
		/// <summary>Convert decimal number array to string dotted address format</summary>
		/// <param name="dec">Address in decimal format at a byte array</param>
		/// <returns>Address in dotted format</returns>
		private string ConvertDec2Str(byte[] dec){
			byte decLength = (byte) dec.Length ;
			string strAddress = "" ;
			//concat each dec array element except the last one
			for(byte i=0; i<decLength; i++){
				strAddress += dec[i].ToString() ;
				if(i<decLength-1){
					strAddress += "." ;
				}
			}
			return strAddress ;
		}
		
		/// <summary>
		/// Calculate the host capacity of the network based on netmask length.
		/// The maximum capacity based on the netmask rules is (2^(32-24))-2 = 16,777,214 hosts.
		/// </summary>
		/// <param name="netmaskLength">A valid netmask length value (from 8 to 30)</param>
		/// <returns>The network hosts capacity</returns>
		private int CalcNetCapacity(byte netmaskLength){
			int capacity = (int) (Math.Pow(2, (32-netmaskLength)) - 2) ;
			return capacity ;
		}
		
	}
}
