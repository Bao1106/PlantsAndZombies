//
//  BiometricAuthentication.swift
//  BiometricAuthentication
//
//  Created by Pham Thanh Dat on 2/18/20.
//  Copyright © 2020 Pham Thanh Dat. All rights reserved.
//

import Foundation
import UIKit
import LocalAuthentication

@objc public class BiometricAuthentication: NSObject {
    
    @objc public static let shared = BiometricAuthentication()
  
    var valueExtra:String = ""
    var unityObject:String = ""
    var signMessage = ""
    
//    private static var shared = BiometricAuthentication()
//    @objc public static func getInstance() -> BiometricAuthentication {
//        return shared
//    }
    
    @objc public func StartBiometricAuthentication() -> Void {
        
        print("hello there!.. StartBiometricAuthentication")
        valueExtra = BiometricConstant.TYPE_BIO_AUTHENTICATION
        BioAuthen()
    }
    @objc public func StartBiometricAuthenticationCustom(_ UnityObjectName: String) -> Void {
        
        print("hello there!.. StartBiometricAuthenticationCustom")
        valueExtra = BiometricConstant.TYPE_BIO_AUTHENTICATION_CUSTOM
        unityObject = UnityObjectName
        BioAuthen()
    }
    @objc public func StartBiometricPublicKeyBioAuthentication() -> Void {
        print("hello there!.. StartBiometricPublicKeyBioAuthentication")
        valueExtra = BiometricConstant.TYPE_PUBLIC_KEY_BIO_AUTHENTICATION
        BioAuthen()
    }
    @objc public func DeleteBiometricPublicKeyBioAuthentication() -> Void {
        print("hello there!.. DeleteBiometricPublicKeyBioAuthentication")
        valueExtra = BiometricConstant.TYPE_DELETE_PUBLIC_KEY_AUTHENTICATION
        BioAuthen()
    }
    
    @objc public func StartBiometricSignatureCodeAuthentication(_ Message:String) -> Void {
        
        print("hello there!.. StartBiometricSignatureCodeAuthentication")
        valueExtra = BiometricConstant.TYPE_SIGNATURE_CODE_AUTHENTICATION
        signMessage = Message
        //BioAuthen()
        self.SendSuccessMessage(valueExtra: self.valueExtra)
    }
    
   @objc public func JsonResultString(ResultCode:String, FuntionCallback:String, Value:String ) -> String {
        let para:NSMutableDictionary = NSMutableDictionary()
        para.setValue(ResultCode, forKey: "result_code")
        para.setValue(FuntionCallback, forKey: "callback_name")
        para.setValue(Value, forKey: "value")
        
        let jsonData = try! JSONSerialization.data(withJSONObject: para)
        let jsonString = String(data: jsonData, encoding: String.Encoding.utf8)!
        return jsonString
    }
    
   @objc public func SendFailedMessage(valueExtra:String) -> Void {
        switch valueExtra {
        case BiometricConstant.TYPE_BIO_AUTHENTICATION:
            let resultBio = JsonResultString(ResultCode: "0", FuntionCallback: BiometricConstant.AUTHENTICATION_BIO_CALLBACK, Value: "")
            SendMessageToUnity(ObjectInUnity: BiometricConstant.BIO_MANAGER_OBJECT, FunctionCallback: BiometricConstant.AUTHENTICATION_BIO_CALLBACK, Result: resultBio)
            break
        case BiometricConstant.TYPE_BIO_AUTHENTICATION_CUSTOM:
            let resultBioCustom = JsonResultString(ResultCode: "0", FuntionCallback: BiometricConstant.AUTHENTICATION_BIO_CALLBACK, Value: "")
            SendMessageToUnity(ObjectInUnity: unityObject, FunctionCallback: BiometricConstant.AUTHENTICATION_BIO_CUSTOM_CALLBACK, Result: resultBioCustom)
            break
        case BiometricConstant.TYPE_PUBLIC_KEY_BIO_AUTHENTICATION:
             print("Unity - Start Biometric And Get PublicKey BioAuth");
            let resultBioPublicKey = JsonResultString(ResultCode: "0", FuntionCallback: BiometricConstant.AUTHENTICATION_BIO_PUBLIC_KEY_CALLBACK, Value: "")
             SendMessageToUnity(ObjectInUnity: BiometricConstant.BIO_MANAGER_OBJECT, FunctionCallback: BiometricConstant.AUTHENTICATION_BIO_PUBLIC_KEY_CALLBACK, Result: resultBioPublicKey)
            break
        case BiometricConstant.TYPE_SIGNATURE_CODE_AUTHENTICATION:
             print("Unity - Start Biometric And Get Signature code BioAuth");
            let resultBioSignatureCode  = JsonResultString(ResultCode: "0", FuntionCallback: BiometricConstant.AUTHENTICATION_BIO_SIGNATURE_CODE_CALLBACK, Value: "")
             SendMessageToUnity(ObjectInUnity: BiometricConstant.BIO_MANAGER_OBJECT, FunctionCallback: BiometricConstant.AUTHENTICATION_BIO_SIGNATURE_CODE_CALLBACK, Result: resultBioSignatureCode)
            break
        case BiometricConstant.TYPE_DELETE_PUBLIC_KEY_AUTHENTICATION:
             print("Unity - Delete Biometric public key BioAuth");
            let resultDeletePublicKeyBio  = JsonResultString(ResultCode: "0", FuntionCallback: BiometricConstant.AUTHENTICATION_BIO_DELETE_PUBLIC_KEY_CALLBACK, Value: "")
            SendMessageToUnity(ObjectInUnity: BiometricConstant.BIO_MANAGER_OBJECT, FunctionCallback: BiometricConstant.AUTHENTICATION_BIO_SIGNATURE_CODE_CALLBACK, Result: resultDeletePublicKeyBio)
                break
        default:
            break
        }
    }
    
   @objc public func SendSuccessMessage(valueExtra:String) -> Void {
        switch valueExtra {
        case BiometricConstant.TYPE_BIO_AUTHENTICATION:
             let resultBio = JsonResultString(ResultCode: "1", FuntionCallback: BiometricConstant.AUTHENTICATION_BIO_CALLBACK, Value: "")
             SendMessageToUnity(ObjectInUnity: BiometricConstant.BIO_MANAGER_OBJECT, FunctionCallback: BiometricConstant.AUTHENTICATION_BIO_CALLBACK, Result: resultBio)
            break
        case BiometricConstant.TYPE_BIO_AUTHENTICATION_CUSTOM:
             let resultBioCustom = JsonResultString(ResultCode: "1", FuntionCallback: BiometricConstant.AUTHENTICATION_BIO_CALLBACK, Value: "")
             SendMessageToUnity(ObjectInUnity: unityObject, FunctionCallback: BiometricConstant.AUTHENTICATION_BIO_CUSTOM_CALLBACK, Result: resultBioCustom)
            break
        case BiometricConstant.TYPE_PUBLIC_KEY_BIO_AUTHENTICATION:
             NSLog("Unity - Start Biometric And Get PublicKey BioAuth")
             var publicKey = ""
             do {
                publicKey = try Manager.shared.publicKey()
                NSLog("Public key \(publicKey)")
             }
             catch let error {
                NSLog("Error \(error)")
             }
             let resultBioPublicKey = JsonResultString(ResultCode: "1", FuntionCallback: BiometricConstant.AUTHENTICATION_BIO_PUBLIC_KEY_CALLBACK, Value: publicKey)
             SendMessageToUnity(ObjectInUnity: BiometricConstant.BIO_MANAGER_OBJECT, FunctionCallback: BiometricConstant.AUTHENTICATION_BIO_PUBLIC_KEY_CALLBACK, Result: resultBioPublicKey)
            break
        case BiometricConstant.TYPE_SIGNATURE_CODE_AUTHENTICATION:
            NSLog("Unity - Start Biometric And Get Signature code BioAuth");
            var signatureBase64 = ""
            do {
                let input = signMessage.data(using: .utf8) ?? Data()
                let signature = try Manager.shared.sign(input)
                signatureBase64 = signature.base64EncodedString()
                NSLog("TYPE_SIGNATURE_CODE_AUTHENTICATION Success: Signature \(signatureBase64)")
            }
            catch let error {
                NSLog("Error \(error)")
            }
             
            let resultBioSignatureCode  = JsonResultString(ResultCode: "1", FuntionCallback: BiometricConstant.AUTHENTICATION_BIO_SIGNATURE_CODE_CALLBACK, Value: signatureBase64)
            SendMessageToUnity(ObjectInUnity: BiometricConstant.BIO_MANAGER_OBJECT, FunctionCallback: BiometricConstant.AUTHENTICATION_BIO_SIGNATURE_CODE_CALLBACK, Result: resultBioSignatureCode)
            break
        case BiometricConstant.TYPE_DELETE_PUBLIC_KEY_AUTHENTICATION:
            do
            {
                try Manager.shared.deleteKeyPair()
            }
            catch let error {
                NSLog("Error \(error)")
            }
            let resultDeletePublicKeyBio  = JsonResultString(ResultCode: "1", FuntionCallback: BiometricConstant.AUTHENTICATION_BIO_DELETE_PUBLIC_KEY_CALLBACK, Value: "")
            SendMessageToUnity(ObjectInUnity: BiometricConstant.BIO_MANAGER_OBJECT, FunctionCallback: BiometricConstant.AUTHENTICATION_BIO_SIGNATURE_CODE_CALLBACK, Result: resultDeletePublicKeyBio)
                break
        default:
            break
        }
    }

    
    @objc public func BioAuthen() -> Void {
        let myContext = LAContext()
        myContext.localizedFallbackTitle = "Please use your Passcode"
        let myLocalizedReasonString = "Biometric Authentication!"

        var authError: NSError?

        if let currentViewController = BiometricAuthentication.GetGLViewController() {

            if #available(iOS 9.0, macOS 10.12.1, *) {
                guard myContext.canEvaluatePolicy(LAPolicy.deviceOwnerAuthentication, error: &authError) else {
                    self.SendFailedMessage(valueExtra: self.valueExtra)
                    return
                }
                if myContext.canEvaluatePolicy(LAPolicy.deviceOwnerAuthentication, error: &authError){
                    myContext.evaluatePolicy(LAPolicy.deviceOwnerAuthentication, localizedReason: myLocalizedReasonString){(success, evaluateError) in
                        DispatchQueue.main.async {
                            if success {
                                self.SendSuccessMessage(valueExtra: self.valueExtra)
                                // User authenticated successfully, take appropriate action
//                                let ac = UIAlertController(title: "Authentication successfully", message: "Awesome!!... User authenticated successfully", preferredStyle: .alert)
//                                ac.addAction(UIAlertAction(title: "OK", style: .default))
//                                currentViewController.present(ac, animated: true)
                                
                            } else {
                                self.SendFailedMessage(valueExtra: self.valueExtra)
                                let ac = UIAlertController(title: "Authentication failed", message: "Sorry!!... User did not authenticate successfully", preferredStyle: .alert)
                                ac.addAction(UIAlertAction(title: "OK", style: .default))
                                currentViewController.present(ac, animated: true)
                               
                            }
                        }
                    }
                }
            }else {
                // Fallback on earlier versions
                let ac = UIAlertController(title: "Touch ID not available", message: "Ooops!!.. This feature is not supported.", preferredStyle: .alert)
                ac.addAction(UIAlertAction(title: "OK", style: .default))
                currentViewController.present(ac, animated: true)
                self.SendFailedMessage(valueExtra: self.valueExtra)
            }
        }
    }
    
    /**
     Bridges UnitySendMessage.
     */
    @objc public func SendMessageToUnity(ObjectInUnity: String, FunctionCallback:String, Result:String) -> Void {
           print("Object In Unity : " + ObjectInUnity + " == Function Callback: " + FunctionCallback + " == Result: " + Result)
           UnitySendMessage(ObjectInUnity, FunctionCallback, Result)
       }

    /**
     Bridges UnityGetGLViewController
     */
   @objc public class func GetGLViewController() -> UIViewController! {
        return UnityGetGLViewController()
    }
}

