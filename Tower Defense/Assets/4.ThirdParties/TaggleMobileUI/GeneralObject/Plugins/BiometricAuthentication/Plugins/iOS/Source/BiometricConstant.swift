//
//  BiometricConstant.swift
//  BiometricAuthentication
//
//  Created by Pham Thanh Dat on 2/18/20.
//  Copyright © 2020 Pham Thanh Dat. All rights reserved.
//

import Foundation

public class BiometricConstant
{
    public static let KEY_NAME = "Bio" //UUID.randomUUID().toString();

    public static let KEY_EXTRA = "EXTRA"
    public static let KEY_EXTRA_SIGN = "EXTRA_SIGN"
    public static let TYPE_BIO_AUTHENTICATION = "BIO_AUTHENTICATION"
    public static let TYPE_BIO_AUTHENTICATION_CUSTOM = "BIO_AUTHENTICATION_CUSTOM"
    public static let TYPE_PUBLIC_KEY_BIO_AUTHENTICATION = "GET_PUBLIC_KEY_BIO_AUTHENTICATION"
    public static let TYPE_SIGNATURE_CODE_AUTHENTICATION = "GET_SIGNATURE_CODE_BIO_AUTHENTICATION"
    public static let TYPE_DELETE_PUBLIC_KEY_AUTHENTICATION = "DELETE_PUBLIC_KEY_BIO_AUTHENTICATION"
    public static let KEY_BIO_MANAGER_OBJECT_CUSTOM = "EXTRA_MANAGER_OBJECT_CUSTOM"

    public static let BIO_MANAGER_OBJECT = "GeneralObject/BiometricManager"
    public static let AUTHENTICATION_BIO_CALLBACK = "AuthenticationBioCallback"
    public static let AUTHENTICATION_BIO_CUSTOM_CALLBACK = "AuthenticationBioCustomCallback"
    public static let AUTHENTICATION_BIO_PUBLIC_KEY_CALLBACK = "AuthenticationBioPublicKeyCallback"
    public static let AUTHENTICATION_BIO_SIGNATURE_CODE_CALLBACK = "AuthenticationBioSignatureCodeCallback"
    public static let AUTHENTICATION_BIO_DELETE_PUBLIC_KEY_CALLBACK = "AuthenticationBioDeletePublicKeyCallback"

    public static let ALGORITHM_R1 = "secp256r1"
    public static let ALGORITHM_K1 = "secp256k1"
    public static let HASH_ENCRYPTION_ALGORITHM_256 = "SHA256withECDSA"
    public static let ENCRYPTION_ALGORITHM_EC = "EC"
    public static let CHARSET_UTF8 = "utf-8"
}
