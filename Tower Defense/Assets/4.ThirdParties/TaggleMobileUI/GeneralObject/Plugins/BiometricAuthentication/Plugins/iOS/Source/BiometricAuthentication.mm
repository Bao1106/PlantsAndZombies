#import <Foundation/Foundation.h>
#include <UnityFramework/UnityFramework-Swift.h>
#pragma mark - C interface

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

extern "C" {
    void _startBiometricAuthentication() {
        [[BiometricAuthentication shared] StartBiometricAuthentication];
    }

    void _startBiometricAuthenticationCustom(const char* unityObject) {
        NSString *unityObjectName = CreateNSString(unityObject);
        [[BiometricAuthentication shared] StartBiometricAuthenticationCustom:unityObjectName];
    }

    void _startBiometricPublicKey() {
        [[BiometricAuthentication shared] StartBiometricPublicKeyBioAuthentication];
    }

    void _startBiometricSignatureCode(const char* signMessage) {
        NSString *signString = CreateNSString(signMessage);
        [[BiometricAuthentication shared] StartBiometricSignatureCodeAuthentication:signString];
    }

    void _deleteBiometricPublicKey() {
        [[BiometricAuthentication shared] DeleteBiometricPublicKeyBioAuthentication];
    }
}
